namespace EInvoice.Common.Pagination
{
    public static class ValidateHelper
    {
        public static bool Validate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue || dateTime == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        public static bool Validate(this int? value)
        {
            if (!value.HasValue || value == 0)
            {
                return false;
            }

            return true;
        }

        public static bool Validate(this int value)
        {
            if (value == 0)
            {
                return false;
            }

            return true;
        }

        public static bool Validate(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        public static bool BoolValidate(this string data)
        {
            try
            {
                bool flag = Convert.ToBoolean(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool BoolValidate(this int data)
        {
            try
            {
                bool flag = Convert.ToBoolean(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool BoolValidate(this short data)
        {
            try
            {
                bool flag = Convert.ToBoolean(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool BoolValidate(this long data)
        {
            try
            {
                bool flag = Convert.ToBoolean(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
