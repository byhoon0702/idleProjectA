using UnityEngine;
using System.Collections;

public class AFInAppEvents
{
	/**
	 * Event Type
	 * */
	public const string LEVEL_ACHIEVED = "af_level_achieved";
	public const string ADD_PAYMENT_INFO = "af_add_payment_info";
	public const string ADD_TO_CART = "af_add_to_cart";
	public const string ADD_TO_WISH_LIST = "af_add_to_wishlist";
	public const string COMPLETE_REGISTRATION = "af_complete_registration";
	public const string TUTORIAL_COMPLETION = "af_tutorial_completion";
	public const string INITIATED_CHECKOUT = "af_initiated_checkout";
	public const string PURCHASE = "af_purchase";
	public const string RATE = "af_rate";
	public const string SEARCH = "af_search";
	public const string SPENT_CREDIT = "af_spent_credits";
	public const string ACHIEVEMENT_UNLOCKED = "af_achievement_unlocked";
	public const string CONTENT_VIEW = "af_content_view";
	public const string TRAVEL_BOOKING = "af_travel_booking";
	public const string SHARE = "af_share";
	public const string INVITE = "af_invite";
	public const string LOGIN = "af_login";
	public const string RE_ENGAGE = "af_re_engage";
	public const string UPDATE = "af_update";
	public const string OPENED_FROM_PUSH_NOTIFICATION = "af_opened_from_push_notification";
	public const string LOCATION_CHANGED = "af_location_changed";
	public const string LOCATION_COORDINATES = "af_location_coordinates";
	public const string ORDER_ID = "af_order_id";
	public const string STAGE_CLEAR = "af_stage_clear";
	public const string NICKNAME_CREATE = "af_nickname_create";
	public const string AD_VIEW_FREE_HEART = "af_ad_view_freeheart";
	public const string AD_VIEW_FREE_DIAMOND = "af_ad_view_freediamond";
	public const string AD_VIEW_FREE_GOLD = "af_ad_view_freegold";
	public const string AD_VIEW_RESULT_CODE = "af_ad_view_resultcode";

	public const string TERMS_OPENED = "af_01_terms_opened";
	public const string TERMS_AGREED = "af_02_terms_agreed";
	public const string TERMS_SUCCESS = "af_03_terms_success";

	public const string DOWNLOAD_OPENED = "af_04_zip_download_opened";
	public const string DOWNLOAD_CLICKED = "af_05_zip_download_clicked";
	public const string DOWNLOAD_COMPLETED = "af_06_zip_download_completed";
	public const string SHOW_PLATFORM_BUTTON = "af_07_login_button_clicked";
	public const string PLATFORM_CLICKED = "af_08_registration_completed";
	public const string NOTICE_OPENED = "af_09_notice_opened";
	public const string NOTICE_CLICKED = "af_10_notice_closed";
	public const string SHOW_LOGIN_BUTTON = "af_11_login_02_completed";
	public const string LOBBY_CLICKED = "af_12_login_03_completed";

	public const string TUTORIAL_START = "af_tutorial_started";
	public const string TUTORIAL_NICKNAME_OPENED = "af_tutorial_nickname_opened";
	public const string TUTORIAL_NICKNAME_COMPLETED = "af_tutorial_nickname_completed";

	public const string TUTORIAL_START_STEP = "af_tutorial_start_step";
	public const string TUTORIAL_COMPLETE_STEP = "af_tutorial_complete_step";
	public const string TUTORIAL_COMPLETE = "af_tutorial_completed";

	public const string MOVIE_START = "af_15_intro_start";
	public const string MOVIE_SKIP = "af_16_intro_skip";
	public const string MOVIE_END = "af_17_intro_complete";

	public const string PART_1_START = "af_18_part_1_start";
	public const string PART_1_TOUCH = "af_19_part_1_touch";

	public const string NICK_NAME_STORY_OPEN = "af_20_nick_01_start";
	public const string NICK_NAME_CREATE = "af_21_nick_02_complete";
	public const string NICK_NAME_STORY_CLOSE = "af_22_lobby_enter";

	public const string CHAPTER_COMPLETE = "af_chapter_complete";
	public const string CHAPTER_FAIL = "af_chapter_defeat";
	public const string CHAPTER_STORY_SKIP = "af_chapter_story_skip";

	/**
	 * Event Parameter Name
	 * **/
	public const string ACCOUNT_ID = "af_account_id";
	public const string NICKNAME = "af_nickname";
	public const string PLATFORM = "af_platform";
	public const string LEVEL = "af_level";
	public const string SCORE = "af_score";
	public const string SUCCESS = "af_success";
	public const string PRICE = "af_price";
	public const string CONTENT_TYPE = "af_content_type";
	public const string CONTENT_ID = "af_content_id";
	public const string CONTENT_LIST = "af_content_list";
	public const string CURRENCY = "af_currency";
	public const string QUANTITY = "af_quantity";
	public const string REGSITRATION_METHOD = "af_registration_method";
	public const string PAYMENT_INFO_AVAILIBLE = "af_payment_info_available";
	public const string MAX_RATING_VALUE = "af_max_rating_value";
	public const string RATING_VALUE = "af_rating_value";
	public const string SEARCH_STRING = "af_search_string";
	public const string DATE_A = "af_date_a";
	public const string DATE_B = "af_date_b";
	public const string DESTINATION_A = "af_destination_a";
	public const string DESTINATION_B = "af_destination_b";
	public const string DESCRIPTION = "af_description";
	public const string CLASS = "af_class";
	public const string EVENT_START = "af_event_start";
	public const string EVENT_END = "af_event_end";
	public const string LATITUDE = "af_lat";
	public const string LONGTITUDE = "af_long";
	public const string CUSTOMER_USER_ID = "af_customer_user_id";
	public const string VALIDATED = "af_validated";
	public const string REVENUE = "af_revenue";
	public const string RECEIPT_ID = "af_receipt_id";
	public const string PARAM_1 = "af_param_1";
	public const string PARAM_2 = "af_param_2";
	public const string PARAM_3 = "af_param_3";
	public const string PARAM_4 = "af_param_4";
	public const string PARAM_5 = "af_param_5";
	public const string PARAM_6 = "af_param_6";
	public const string PARAM_7 = "af_param_7";
	public const string PARAM_8 = "af_param_8";
	public const string PARAM_9 = "af_param_9";
	public const string PARAM_10 = "af_param_10";
}
