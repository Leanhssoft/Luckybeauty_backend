using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Consts
{
    public class ZaloTemplateType
    {
        public const string PROMOTION = "promotion";
        public const string EVENT = "transaction_event";
        public const string TRANSACTION = "transaction_transaction";
        public const string BOOKING = "transaction_booking";
        public const string PARTNERSHIP = "transaction_partnership";
        public const string MEMBERSHIP = "transaction_membership";
        public const string MEDIA = "media";
    }
    public class ZaloElementType
    {
        public const string BANNER = "banner";
        public const string HEADER = "header";
        public const string TABLE = "table";
        public const string TEXT = "text";
        public const string IMAGE = "image";
    }
    public class ZaloButtonType
    {
        public const string URL = "oa.open.url";
        public const string PHONE = "oa.open.phone";
        public const string SHOW = "oa.query.show";
    }
}
