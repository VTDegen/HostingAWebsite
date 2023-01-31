using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSNofication.Areas.Transaction.Models
{
    public class RequestNotificationData
    {
        public string UserName {get;set;}
        public string UserRole { get;set;}
        public string Gender { get;set;}
        public string CivilStatus { get;set;}
        public string PostBody { get;set;}
        public string PostAttachment { get;set;}
        public string PostContains { get;set;}
        public string SrchUsername { get; set; }
        public string SrchFirstName { get; set; }
        public string SrchLastName { get; set; }
        public string SrchMiddleName { get; set; }
        public string SrchContactNumber { get; set; }
        public string SrchEmail { get; set; }
        public string SrchGender { get; set; }
        public string SrchCivilStatus { get; set; }
        public string SrchRole { get; set; }
        public string SrchStuDepartment { get; set; }
        public string SrchStuCourse { get; set; }
        public string SrchStuSection { get; set; }
        public string SrchStuYearLevel { get; set; }
        public string SrchTES { get; set; }
        public string SrchTDP { get; set; }
        public string SrchPersonDisability { get; set; }
        public string SrchWorkingStudent { get; set; }
        public string SrchNotApplicable { get; set; }
        public string SrchOrganization { get; set; }
        public string SrchALLS { get; set; }
        public string SrchAPL { get; set; }
        public string SrchASGRD { get; set; }
        public string SrchCAITO { get; set; }
        public string SrchCAPS { get; set; }
        public string SrchCSS { get; set; }
        public string SrchDCP { get; set; }
        public string SrchENC { get; set; }
        public string SrchEXCEL { get; set; }
        public string SrchFELTA { get; set; }
        public string SrchIFIGHT { get; set; }
        public string SrchITS { get; set; }
        public string SrchJPIA { get; set; }
        public string SrchLAGABATA { get; set; }
        public string SrchLEAD { get; set; }
        public string SrchMDAS { get; set; }
        public string SrchMSS { get; set; }
        public string SrchROTARACT { get; set; }
        public string SrchSAVE { get; set; }
        public string SrchSFJ { get; set; }
        public string SrchSOCIOS { get; set; }
        public string SrchPROTEGE { get; set; }
        public string SrchUM3P { get; set; }
        public string SrchFacDepartment { get; set; }
        public string SrchFacPosition { get; set; }
    }
    public class SendAnnouncement
    {
        public int ID { get; set; }
        public int UserID { get; set; }
      public string Announcement { get; set; }
      public string attachmentName { get; set; }
      public string DatePosted { get; set; }
      public string PostBody { get; set; }
      public string PostedBy { get; set; }
      public bool IsDeleted { get; set; }
        public string CreateID { get; set; }
        public string CreateDate { get; set; }
        public string UpdateID { get; set; }
        public string UpdateDate { get; set; }

    }
    public class AnnouncementList {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string UserRole { get; set; }
        public string Announcement { get; set; }
        public string attachmentName { get; set; }
        public string DatePosted { get; set; }
        public string PostBody { get; set; }
        public string PostedBy { get; set; }
    }
}