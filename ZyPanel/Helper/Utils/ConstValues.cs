namespace ZyPanel.Helper.Utils {
    public static class ConstValues {
        // Numeral
        public const int PAGE_SIZE = 10;
        public const int UPLOAD_SIZE_IMAGE_IN_MB = 5 * 1024 * 1024;

        // Ok
        public const string OkSave = "اطلاعات با موفقیت ذخیره گردید.";
        public const string OkRemove = "اطلاعات با موفقیت حذف گردید.";
        public const string OkUpdate = "اطلاعات با موفقیت ویرایش گردید.";
        public const string OkRequest = "درخواست شما با موفقیت انجام گردید.";

        // Error
        public const string ErrRemove = "درخواست شما با خطا مواجه شد.";
        public const string ErrRequest = "درخواست شما با خطا مواجه شد.";
        public const string ErrOnDependency = "It has some dependencies";
        public const string ErrOnExistData = "درخواست شما پیشتر ثبت شده است.";
        public const string ErrOnFetchingData = "Some error occured on fetching data.";

        // Required
        public const string RqError = "اجباری می باشد";

        public const string RgError = "معتبر نمی باشد";
    }
}