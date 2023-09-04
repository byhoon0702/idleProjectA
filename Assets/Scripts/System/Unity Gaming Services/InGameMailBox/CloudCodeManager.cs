using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using Unity.Services.Authentication;
using System;

namespace InGameMailBox
{
	public class CloudCodeManager : MonoBehaviour
	{
		public const int k_CloudCodeRateLimitExceptionStatusCode = 50;
		public const int k_CloudCodeMissingScriptExceptionStatusCode = 9002;
		public const int k_CloudCodeUnprocessableEntityExceptionStatusCode = 9009;

		// HTTP REST API status codes
		public const int k_HttpBadRequestStatusCode = 400;
		public const int k_HttpTooManyRequestsStatusCode = 429;

		// Custom status codes
		public const int k_UnexpectedFormatCustomStatusCode = int.MinValue;
		public const int k_VirtualPurchaseFailedStatusCode = 2;
		public const int k_MissingCloudSaveDataStatusCode = 3;
		public const int k_InvalidArgumentStatusCode = 4;
		public const int k_AttachmentAlreadyClaimedStatusCode = 5;
		public const int k_NoClaimableAttachmentsStatusCode = 6;

		// Unity Gaming Services status codes via Cloud Code
		public const int k_EconomyValidationExceptionStatusCode = 1007;
		public const int k_RateLimitExceptionStatusCode = 50;
		public static CloudCodeManager Instance { get; private set; }


		private void Awake()
		{

			Instance = this;

		}



		public async Task CallClaimMessageAttachmentEndpoint(string messageId)
		{
			try
			{
				Debug.Log($"Claiming attachment for message {messageId} via Cloud Code...");

				await CloudCodeService.Instance.CallEndpointAsync("InGameMailbox_ClaimAttachment",
					new Dictionary<string, object> { { "messageId", messageId } });
			}
			catch (CloudCodeException e)
			{
				HandleCloudCodeException(e);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
			finally
			{
				if (this != null)
				{

				}
			}
		}
		public async Task CallClaimAllMessageAttachmentsEndpoint()
		{
			try
			{
				Debug.Log("Claiming all message attachments via Cloud Code...");

				var result = await CloudCodeService.Instance.CallEndpointAsync<ClaimAllResult>(
					 "InGameMailbox_ClaimAllAttachments", new Dictionary<string, object>());
				if (this == null) return;

			}
			catch (CloudCodeException e)
			{
				HandleCloudCodeException(e);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		public async Task CallResetEventEndpoint()
		{
			ThrowIfNotSignedIn();
			try
			{
				await CloudCodeService.Instance.CallEndpointAsync("DailyRewards_ResetEvent", new Dictionary<string, object>());
			}
			catch (CloudCodeException ex)
			{
				HandleCloudCodeException(ex);
				throw new CloudCodeResultUnavailableException(ex,
						  "Handled exception in 'CallResetEventEndpoint.'");
			}
		}


		void ThrowIfNotSignedIn()
		{
			if (!AuthenticationService.Instance.IsSignedIn)
			{
				Debug.LogError("Cloud Code can't be called because you're not signed in.");

				throw new CloudCodeResultUnavailableException(null,
					"Not signed in to authentication in 'CloudCodeManager.'");
			}
		}
		public void HandleCloudCodeException(CloudCodeException e)
		{
			switch (e.ErrorCode)
			{
				case k_CloudCodeUnprocessableEntityExceptionStatusCode:
					var cloudCodeCustomError = ConvertToActionableError(e);
					HandleCloudCodeScriptError(cloudCodeCustomError);
					break;

				case k_CloudCodeRateLimitExceptionStatusCode:
					Debug.Log("Cloud Code exceeded its Rate Limit. Try Again.");
					break;

				case k_CloudCodeMissingScriptExceptionStatusCode:
					Debug.Log("Couldn't find requested Cloud Code Script.");
					break;

				default:
					// Handle other native client errors
					Debug.Log("Error Code: " + e.ErrorCode);
					Debug.Log(e);
					break;
			}
		}
		static CloudCodeCustomError ConvertToActionableError(CloudCodeException e)
		{
			try
			{
				// extract the JSON part of the exception message
				var trimmedMessage = e.Message;
				trimmedMessage = trimmedMessage.Substring(trimmedMessage.IndexOf('{'));
				trimmedMessage = trimmedMessage.Substring(0, trimmedMessage.LastIndexOf('}') + 1);

				// Convert the message string ultimately into the Cloud Code Custom Error object which has a
				// standard structure for all errors.
				return JsonUtility.FromJson<CloudCodeCustomError>(trimmedMessage);
			}
			catch (Exception exception)
			{
				return new CloudCodeCustomError("Failed to Parse Error", k_UnexpectedFormatCustomStatusCode,
					"Cloud Code Unprocessable Entity exception is in an unexpected format and " +
					$"couldn't be parsed: {exception.Message}", e);
			}
		}

		void HandleCloudCodeScriptError(CloudCodeCustomError cloudCodeCustomError)
		{
			switch (cloudCodeCustomError.status)
			{
				case k_VirtualPurchaseFailedStatusCode:
				case k_MissingCloudSaveDataStatusCode:
				case k_InvalidArgumentStatusCode:
					//sceneView.ShowClaimFailedPopup("Failed to Claim Attachment", cloudCodeCustomError.message);
					break;

				case k_AttachmentAlreadyClaimedStatusCode:
					//sceneView.ShowClaimFailedPopup("Attachment Already Claimed", cloudCodeCustomError.message);
					break;

				case k_NoClaimableAttachmentsStatusCode:
					//sceneView.ShowClaimFailedPopup("No Attachments to Claim", cloudCodeCustomError.message);
					break;

				case k_EconomyValidationExceptionStatusCode:
				case k_HttpBadRequestStatusCode:
					Debug.Log("A bad server request occurred during Cloud Code script execution: " +
								$"{cloudCodeCustomError.name}: {cloudCodeCustomError.message} : " +
								$"{cloudCodeCustomError.details[0]}");
					break;

				case k_RateLimitExceptionStatusCode:
					// With this status code, message will include which service triggered this rate limit.
					Debug.Log($"{cloudCodeCustomError.message}. Wait {cloudCodeCustomError.retryAfter} " +
								$"seconds and try again.");
					break;

				case k_HttpTooManyRequestsStatusCode:
					Debug.Log($"Rate Limit has been exceeded. Wait {cloudCodeCustomError.retryAfter} " +
								$"seconds and try again.");
					break;

				case k_UnexpectedFormatCustomStatusCode:
					Debug.Log($"Cloud Code returned an Unprocessable Entity exception, " +
								$"but it could not be parsed: {cloudCodeCustomError.message}. " +
								$"Original error: {cloudCodeCustomError.InnerException?.Message}");
					break;

				default:
					Debug.Log($"Cloud code returned error: {cloudCodeCustomError.status}: " +
								$"{cloudCodeCustomError.name}: {cloudCodeCustomError.message}");
					break;
			}
		}
		private void OnDestroy()
		{
			Instance = null;
		}

		public struct ClaimAllResult
		{
			public string[] processedTransactions;
		}

		class CloudCodeCustomError : Exception
		{
			public int status;
			public string name;
			public string message;
			public string retryAfter;
			public string[] details;

			public CloudCodeCustomError(string name, int status, string message = null,
				Exception innerException = null) : base(message, innerException)
			{
				this.name = name;
				this.status = status;
				this.message = message;
				retryAfter = null;
				details = new string[] { };
			}
		}
	}


}
