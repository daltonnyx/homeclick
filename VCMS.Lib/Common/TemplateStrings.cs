namespace VCMS.Lib.Common
{
    public static class TemplateStrings
    {

        /// <summary>
        /// '0' model name
        /// </summary>
        public const string MODEL_CREATE_SUCCESS = "<strong>{0}</strong> has been created!";

        /// <summary>
        /// '0' model name
        /// </summary>
        public const string MODEL_CREATE_FAIL = "One or more error occurred. <strong>{0}</strong> have not created!";

        /// <summary>
        /// '0' is model type(s)
        /// '1' is models name combined
        /// '2' is model count
        /// </summary>
        public const string MODEL_DELETE_RESULT = "{0} deleted: {1}<br/>Total: {2} items";

        /// <summary>
        /// '0' is models name
        /// </summary>
        public const string MODEL_DELETE_ERROR_UNKNOW = "One or more error occurred. <strong>{0}</strong> were not deleted!";

        /// <summary>
        /// '0' string content
        /// </summary>
        public const string HTML_STRONG = "<strong>{0}</strong>";


    }
}
