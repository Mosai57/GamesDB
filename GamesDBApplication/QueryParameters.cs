namespace GDBAccess
{
    /// <summary>
    /// Object holding GameName, System, and Format to pass to query functions.
    /// </summary>
    class QueryParameters
    {
        public string GameName { get; set; }
        public string System { get; set; }
        public string Format { get; set; }

        public bool ValidateEntry()
        {
            if(GameName == "" || System == "" || Format == "")
            {
                return false;
            }
            return true;
        }
    }
}
