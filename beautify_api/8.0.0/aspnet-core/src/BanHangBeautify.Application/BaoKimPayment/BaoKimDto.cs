using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.BaoKim
{
    public class BaoKimPayMentConst
    {
        public const long AMOUNT_MIN = 10000;
        public const long AMOUNT_MAX = 100000000;
    }

    public class MessageResponse
    {
        public string message { get; set; }
    }

    public class BankInfor
    {
        public string BankName { get; set; }
        public string BankShortName { get; set; }
        public string BankBranch { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; }
        public string Qr { get; set; }
        public string QrPath { get; set; }
    }
    public class BaoKim_CreateQRDto
    {
        [MaxLength(50)]
        public string RequestId { get; set; }
        [MaxLength(50)]
        public string ReferenceId { get; set; }// dùng cho phần tra cứu giao dịch
        [MaxLength(19)]
        public string RequestTime { get; set; }
        [MaxLength(20)]
        public string PartnerCode { get; set; }
        [MaxLength(4)]
        public string Operation { get; set; }
        public int CreateType { get; set; } = 2;
        [MaxLength(50)]
        public string AccName { get; set; }
        [MaxLength(17)]
        public string AccNo { get; set; }
        [MaxLength(25)]
        public string OrderId { get; set; }
        [MaxLength(10)]
        public string ExpireDate { get; set; }

        private string _collectAmountMin = BaoKimPayMentConst.AMOUNT_MIN.ToString();

        public string CollectAmountMin
        {
            get => _collectAmountMin;
            set
            {
                if (long.Parse(value) >= BaoKimPayMentConst.AMOUNT_MIN)
                    _collectAmountMin = value;
                else
                    throw new System.Exception($"Collect amount min ${BaoKimPayMentConst.AMOUNT_MIN} vnd");
            }
        }

        private string _collectAmountMax = BaoKimPayMentConst.AMOUNT_MAX.ToString();

        public string CollectAmountMax
        {
            get => _collectAmountMax;
            set
            {
                if (long.Parse(value) <= BaoKimPayMentConst.AMOUNT_MAX)
                    _collectAmountMax = value;
                else
                    throw new System.Exception($"Collect amount max ${BaoKimPayMentConst.AMOUNT_MAX} vnd");
            }
        }
    }

    public class BaoKim_ResponseQRDto : BaoKim_CreateQRDto
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public AccountInfo AccountInfo { get; set; }
    }
    public class BaoKim_ResponseTraCuuQRDto : BaoKim_CreateQRDto
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public TransactionInfor TransactionList { get; set; }
    }

    public class AccountInfo
    {
        public BankInfor BANK { get; set; }
    }
    public class TransactionInfor
    {
        public int TransAmount { get; set; }
        public string TransTimeBk{ get; set; }
        public string TransIdBk { get; set; }
        public string TransMemo { get; set; }
        public string TransIdPartner { get; set; }
        public string TransTimePartner  { get; set; }
    }

    public class ResponseThongBaoGiaoDich
    {
        public string RequestId { get; set; }
        public string RequestTime { get; set; }
        public string PartnerCode { get; set; }
        public string AccNo { get; set; }
        public string ClientIdNo { get; set; }
        public string TransId { get; set; }
        public int TransAmount { get; set; }
        public string TransTime { get; set; }
        public int BefTransDebt { get; set; }
        public int AffTransDebt { get; set; }
        public int AccountType { get; set; }
        public string OrderId { get; set; }
        public string Signature { get; set; }
    }
    public class CassoResponseThongBaoGiaoDich
    {
        public long privateId { get; set; }
        public string reference { get; set; }
        public string bookingDate { get; set; }
        public string transactionDate { get; set; }
        public long amount { get; set; }
        public string description { get; set; }
        public long runningBalance { get; set; }
        public string virtualAccountNumber { get; set; }
        public string virtualAccountName { get; set; }
        public string paymentChannel { get; set; }
        public string counterAccountNumber { get; set; }
        public string counterAccountName { get; set; }
        public string counterAccountBankId { get; set; }
        public string counterAccountBankName { get; set; }
    }

    public class RequestThongBaoGiaoDich
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ReferenceId { get; set; }
        public string AccNo { get; set; }
        public string Signature { get; set; }
    }
}
