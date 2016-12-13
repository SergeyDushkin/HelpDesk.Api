using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace servicedesk.api
{
    public interface IDbClass
    {
        Guid GUID_RECORD { get; set; }
    }

    public class DbClass : IDbClass
    {
        [Key]
        public Guid GUID_RECORD { get; set; }
    }

    /// <summary>
    /// Список типов локаций
    /// </summary>
    public static class LOCATION_TYPES
    {
        /// <summary>
        /// Сервисная компания
        /// </summary>
        public static Guid Supplier { get { return new Guid("B9A29C64-8702-4981-A8FB-0031F18051CF"); } }

        /// <summary>
        /// Производственная единица
        /// </summary>
        public static Guid PU { get { return new Guid("8F2941EE-B700-426A-8687-091D4E02FB68"); } }

        /// <summary>
        /// Офис
        /// </summary>
        public static Guid Office { get { return new Guid(" 695F0279-DB63-4FA8-BB97-97079167D634"); } }


          /// <summary>
        /// Станция
        /// </summary>
        public static Guid Store { get { return new Guid("C3D11C43-71D9-45AC-BD66-2D34159DFABF"); } }
       
        /// <summary>
        /// Объекты
        /// </summary>
        public static Guid Objects { get { return new Guid("CDEA7734-BC74-43B5-B1CB-47CCC22B3B95"); } }


        /// <summary>
        /// Склад
        /// </summary>
        public static Guid Warehouse { get { return new Guid("A95F27DD-3B26-4DAC-8290-5099F7BDEAD6"); } }

        /// <summary>
        /// Нефтебаза
        /// </summary>
        public static Guid FuelTerminal { get { return new Guid("E90FD327-6754-4601-A31A-50E2E85BEE38"); } }

        /// <summary>
        /// Участок
        /// </summary>
        public static Guid Land { get { return new Guid("C5DDCF51-FADB-4905-9659-5A4188EEA135"); } }

        /// <summary>
        /// Компания
        /// </summary>
        public static Guid Company { get { return new Guid("B5ADF186-1123-4C75-BED4-F5C01A3883F0"); } }

        /// <summary>
        /// Ответственное подразделение
        /// </summary>
        public static Guid RESPONSIBLE_UNIT { get { return new Guid("324514D7-8C2D-4D94-BFAD-6A1FCBBEEC33"); } }

    }

    [Table("WH_LOCATIONS")]
    public class LOCATION : DbClass
    {
        public string LOCATION_NAME { get; set; }
        public Guid LOCATION_TYPE_GUID { get; set; }
        public Guid? LOCATION_OWNER_GUID { get; set; }

        public int LOCATION_LEVEL { get; set; }
        public bool IS_DISABLE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool IS_LEAF { get; set; }

        [ForeignKey("LOCATION_TYPE_GUID")]
        public virtual LOCATION_TYPE LOCATION_TYPE { get; set; }

        [ForeignKey("LOCATION_OWNER_GUID")]
        public virtual LOCATION LOCATION_OWNER { get; set; }

        [ForeignKey("GUID_RECORD")]
        public virtual STORE_ATTR STORE_ATTRS { get; set; }

        [ForeignKey("GUID_RECORD")]
        public virtual LOCATION_SYNC_CODE SYNC { get; set; }

        public virtual LOCATION_CONTACT_INFO CONTACT { get; set; }

        public string GetEmail()
        {
            return CONTACT != null 
                ? CONTACT.EMAIL 
                : string.Empty;
        }

        public LOCATION() { }

        public LOCATION(LOCATION record)
        {
            this.CONTACT = record.CONTACT;
            this.GUID_RECORD = record.GUID_RECORD;
            this.IS_DISABLE = record.IS_DISABLE;
            this.IS_LEAF = record.IS_LEAF;
            this.LOCATION_LEVEL = record.LOCATION_LEVEL;
            this.LOCATION_NAME = record.LOCATION_NAME;
            this.LOCATION_OWNER_GUID = record.LOCATION_OWNER_GUID;
            this.LOCATION_TYPE_GUID = record.LOCATION_TYPE_GUID;
            this.STORE_ATTRS = record.STORE_ATTRS;
            this.SYNC = record.SYNC;

            if (record.LOCATION_OWNER != null)
            {
                this.LOCATION_OWNER = new LOCATION(record.LOCATION_OWNER);
            }

            if (record.LOCATION_TYPE != null)
            {
                this.LOCATION_TYPE = new LOCATION_TYPE
                {
                    GUID_RECORD = record.LOCATION_TYPE.GUID_RECORD,
                    LOCATION_TYPE_NAME = record.LOCATION_TYPE.LOCATION_TYPE_NAME
                };
            }
        }
    }

    [Table("WH_LOCATION_SYNC_CODES")]
    public class LOCATION_SYNC_CODE
    {
        [Key]
        [ForeignKey("LOCATION")]
        public Guid LOCATION_GUID { get; set; }

        public string DOCS_CODE { get; set; }
        public string IBS_CODE { get; set; }
        public string ONSITE_CODE { get; set; }
        public string SUN_CODE { get; set; }

        public virtual LOCATION LOCATION { get; set; }
    }

    [Table("WH_LOCATION_STORE_ATTRS")]
    public class STORE_ATTR
    {
        [Key]
        [ForeignKey("LOCATION")]
        public Guid LOCATION_GUID { get; set; }

        public string RCCODE { get; set; }
        public string DIRECTION { get; set; }

        public Guid? BRAND_GUID { get; set; }
        public Guid? STORE_TYPE_GUID { get; set; }
        public bool? IS_GUARANTEE { get; set; }
        public bool? IS_DISABLE { get; set; }

        [ForeignKey("BRAND_GUID")]
        public virtual BRAND BRAND { get; set; }

        [ForeignKey("STORE_TYPE_GUID")]
        public virtual LOCATION_STORE_TYPES STORE_TYPES { get; set; }

        public virtual LOCATION LOCATION { get; set; }
    }

    [Table("WH_CONTACT_INFO")]
    public class LOCATION_CONTACT_INFO : DbClass
    {
        public Guid REFERENCE_GUID { get; set; }
        public string ADDRESS { get; set; }
        public string EMAIL { get; set; }
        public string MOBPHONE { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public virtual LOCATION REFERENCE { get; set; }
    }

    [Table("WH_LOCATION_TYPES")]
    public class LOCATION_TYPE : DbClass
    {
        public string LOCATION_TYPE_NAME { get; set; }
    }

    [Table("WH_LOCATION_STORE_TYPES")]
    public class LOCATION_STORE_TYPES : DbClass
    {
        public string STORE_TYPE_NAME { get; set; }
    }
    /// <summary>
    /// Бренд
    /// </summary>
    [Table("WH_BRANDS")]
    public class BRAND : DbClass
    {
        /// <summary>
        /// Название бренда
        /// </summary>
        public string BRAND_NAME { get; set; }

        /// <summary>
        /// Код юренда в торгойой системе
        /// </summary>
        public int SYNC_CODE { get; set; }
    }

    [Table("WH_USERS")]
    public class USER : DbClass
    {
        public string FIRST_NAME { get; set; }
        public string SECOND_NAME { get; set; }
        public string LAST_NAME { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FULL_NAME { get; set; }

        public DateTime? BIRTHDAY { get; set; }
        public string EMPLOYEE_NUMBER { get; set; }
        public string POSITION_NAME { get; set; }

        [ForeignKey("LOCATION")]
        public Guid LOCATION_GUID { get; set; }
        public string LOGIN { get; set; }
        public string PASSWORD_HASH { get; set; }
        public bool IS_DISABLE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }

        public LOCATION LOCATION { get; set; }

        [ForeignKey("USER_GUID")]
        public virtual ICollection<USER_ROLE> USER_ROLE { get; set; }

        [ForeignKey("USER_GUID")]
        public virtual ICollection<USER_CONTACT> CONTACTS { get; set; }

        public string GetPhones(string separator = ";")
        {
            var phone = String.Empty;

            if (this.CONTACTS != null)
                if (this.CONTACTS.Any(c => c.TYPE_CODE == 2))
                    phone = String.Join(separator, this.CONTACTS.Where(c => c.TYPE_CODE == 2).Select(c => c.ADDRESS));

            if (phone != null)
                phone = phone.Trim();

            return phone;
        }

        public string GetEmails(string separator = ";")
        {
            var email = String.Empty;

            if (this.CONTACTS != null)
                if (this.CONTACTS.Any(c => c.TYPE_CODE == 1))
                    email = String.Join(separator, this.CONTACTS.Where(c => c.TYPE_CODE == 1).Select(c => c.ADDRESS));

            if (email != null)
                email = email.Trim();

            return email;
        }

        public USER() { }

        public USER(USER record)
        {
            record.CONTACTS = record.CONTACTS;

            this.BIRTHDAY = record.BIRTHDAY;
            this.EMPLOYEE_NUMBER = record.EMPLOYEE_NUMBER;
            this.FIRST_NAME = record.FIRST_NAME;
            this.FULL_NAME = record.FULL_NAME;
            this.GUID_RECORD = record.GUID_RECORD;
            this.IS_DISABLE = record.IS_DISABLE;
            this.LAST_NAME = record.LAST_NAME;
            this.LOCATION_GUID = record.LOCATION_GUID;
            this.PASSWORD_HASH = record.PASSWORD_HASH;
            this.POSITION_NAME = record.POSITION_NAME;
            this.SECOND_NAME = record.SECOND_NAME;
        }
    }

    [Table("WH_ROLES")]
    public class ROLE : DbClass
    {
        public string ROLE_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public bool IS_DISABLE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }

        [ForeignKey("ROLE_GUID")]
        public ICollection<USER_ROLE> USER_ROLE { get; set; } // virtual
    }

    [Table("WH_USER_ROLES")]
    public class USER_ROLE : DbClass
    {
        [ForeignKey("USER")]
        public Guid USER_GUID { get; set; }

        [ForeignKey("ROLE")]
        public Guid ROLE_GUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }

        public USER USER { get; set; } // virtual
        public ROLE ROLE { get; set; } // virtual
    }

    [Table("WH_REQUEST_TIME_CATEGORIES")]
    public class REQUEST_TIME_CATEGORY : DbClass
    {
        public string TIME_CATEGORY_NAME { get; set; }
        public double? TIME { get; set; }
        public bool IS_DISABLE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }
    }

    [Table("WH_REQUESTS")]
    public class REQUEST : DbClass
    {
        /// <summary>
        /// Номер заявки
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int REQUEST_ID { get; set; }

        /// <summary>
        /// Номер короткий
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int REQUEST_NUMBER { get; set; }

        [ForeignKey("PU")]
        public Guid PU_GUID { get; set; }

        [ForeignKey("COMPANY")]
        public Guid COMPANY_GUID { get; set; }

        [ForeignKey("STORE")]
        public Guid STORE_GUID { get; set; }

        [ForeignKey("USER")]
        public Guid USER_GUID { get; set; }

        [ForeignKey("OPERATOR")]
        public Guid? OPERATOR_GUID { get; set; }

        [ForeignKey("CLOSE_USER")]
        public Guid? CLOSE_USER_GUID { get; set; }

        [ForeignKey("CLOSE_OPERATOR")]
        public Guid? CLOSE_OPERATOR_GUID { get; set; }

        [ForeignKey("PROBLEM")]
        public Guid PROBLEM_GUID { get; set; }

        [ForeignKey("LOCATION")]
        public Guid LOCATION_GUID { get; set; }

        [ForeignKey("SETYPE")]
        public Guid SETYPE_GUID { get; set; }

        [ForeignKey("SETYPE2")]
        public Guid? SETYPE2_GUID { get; set; }

        [ForeignKey("CATEGORY")]
        public Guid CATEGORY_GUID { get; set; }

        [ForeignKey("DAMAGE_TYPE")]
        public Guid? DAMAGE_GUID { get; set; }

        public Guid? PREV_REQUEST_GUID { get; set; }

        public DateTimeOffset RequestDate { get; set; }
        public DateTimeOffset? CompleteDate { get; set; }
        public DateTimeOffset DefectDate { get; set; }
        public DateTimeOffset? RepairDate { get; set; }
        public DateTimeOffset? DeadlineDate { get; set; }

        public bool IS_AdditionalWorks { get; set; }
        public bool IS_StaffGuilty { get; set; }
        public bool IS_Guarantee { get; set; }
        public string Comments { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Overdue { get; set; }

        public virtual USER USER { get; set; }
        public virtual USER OPERATOR { get; set; }
        public virtual USER CLOSE_USER { get; set; }
        public virtual USER CLOSE_OPERATOR { get; set; }

        public virtual LOCATION PU { get; set; }
        public virtual LOCATION COMPANY { get; set; }
        public virtual LOCATION STORE { get; set; }

        public virtual LOCATION LOCATION { get; set; }
        public virtual LOCATION PROBLEM { get; set; }
        public virtual LOCATION SETYPE { get; set; }
        public virtual LOCATION SETYPE2 { get; set; }
        public virtual LOCATION DAMAGE_TYPE { get; set; }

        public virtual REQUEST_TIME_CATEGORY CATEGORY { get; set; }

        [ForeignKey("REQUEST_GUID")]
        public virtual ICollection<REQUEST_JOB> JOBS { get; set; }

        [ForeignKey("REQUEST_GUID")]
        public virtual ICollection<INCIDENT> INCIDENT { get; set; }

        [ForeignKey("REFERENCE_GUID")]
        public virtual ICollection<AUDITOR> AUDITORS { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DATEMODIFY { get; set; }

        public REQUEST() { }

        public REQUEST(REQUEST record)
        {
            this.CATEGORY_GUID = record.CATEGORY_GUID;
            this.CLOSE_OPERATOR_GUID = record.CLOSE_OPERATOR_GUID;
            this.CLOSE_USER_GUID = record.CLOSE_USER_GUID;
            this.Comments = record.Comments;
            this.COMPANY_GUID = record.COMPANY_GUID;
            this.CompleteDate = record.CompleteDate;
            this.DAMAGE_GUID = record.DAMAGE_GUID;
            this.DATECREATE = record.DATECREATE;
            this.DATEMODIFY = record.DATEMODIFY;
            this.DATEUPDATE = record.DATEUPDATE;
            this.DeadlineDate = record.DeadlineDate;
            this.DefectDate = record.DefectDate;
            this.GUID_RECORD = record.GUID_RECORD;
            this.IS_AdditionalWorks = record.IS_AdditionalWorks;
            this.IS_Guarantee = record.IS_Guarantee;
            this.IS_StaffGuilty = record.IS_StaffGuilty;
            this.LOCATION_GUID = record.LOCATION_GUID;
            this.OPERATOR_GUID = record.OPERATOR_GUID;
            this.Overdue = record.Overdue;
            this.PREV_REQUEST_GUID = record.PREV_REQUEST_GUID;
            this.PROBLEM_GUID = record.PROBLEM_GUID;
            this.PU_GUID = record.PU_GUID;
            this.RepairDate = record.RepairDate;
            this.REQUEST_ID = record.REQUEST_ID;
            this.REQUEST_NUMBER = record.REQUEST_NUMBER;
            this.RequestDate = record.RequestDate;
            this.SETYPE_GUID = record.SETYPE_GUID;
            this.SETYPE2_GUID = record.SETYPE2_GUID;
            this.STORE_GUID = record.STORE_GUID;
            this.USER_GUID = record.USER_GUID;
            this.USERCREATE = record.USERCREATE;
            this.USERUPDATE = record.USERUPDATE;

            if (record.CATEGORY != null)
            {
                this.CATEGORY = new REQUEST_TIME_CATEGORY
                {
                    GUID_RECORD = record.CATEGORY.GUID_RECORD,
                    IS_DISABLE = record.CATEGORY.IS_DISABLE,
                    TIME = record.CATEGORY.TIME,
                    TIME_CATEGORY_NAME = record.CATEGORY.TIME_CATEGORY_NAME
                };
            }

            if (record.CLOSE_OPERATOR != null)
            {
                this.CLOSE_OPERATOR = new USER(record.CLOSE_OPERATOR);
            }

            if (record.CLOSE_USER != null)
            {
                this.CLOSE_USER = new USER(record.CLOSE_USER);
            }

            if (record.COMPANY != null)
            {
                this.COMPANY = new LOCATION(record.COMPANY);
            }

            if (record.DAMAGE_TYPE != null)
            {
                this.DAMAGE_TYPE = new LOCATION(record.DAMAGE_TYPE);
            }

            if (record.LOCATION != null)
            {
                this.LOCATION = new LOCATION(record.LOCATION);
            }

            if (record.OPERATOR != null)
            {
                this.OPERATOR = new USER(record.OPERATOR);
            }

            if (record.PROBLEM != null)
            {
                this.PROBLEM = new LOCATION(record.PROBLEM);
            }

            if (record.PU != null)
            {
                this.PU = new LOCATION(record.PU);
            }

            if (record.SETYPE != null)
            {
                this.SETYPE = new LOCATION(record.SETYPE);
            }

            if (record.SETYPE2 != null)
            {
                this.SETYPE2 = new LOCATION(record.SETYPE2);
            }

            if (record.STORE != null)
            {
                this.STORE = new LOCATION(record.STORE);
            }

            if (record.USER != null)
            {
                this.USER = new USER(record.USER);
            }
        }

        public object Clone()
        {
            var copy = new REQUEST(this);

            var s = JsonConvert.SerializeObject(copy,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var r = JsonConvert.DeserializeObject<REQUEST>(s);
            return r;
        }
    }

    [Table("WH_REQUEST_JOBS")]
    public class REQUEST_JOB : DbClass
    {
        [ForeignKey("REQUEST")]
        public Guid REQUEST_GUID { get; set; }

        [ForeignKey("SUPPLIER")]
        public Guid? SUPPLIER_GUID { get; set; }

        [ForeignKey("USER")]
        public Guid? USER_GUID { get; set; }

        public DateTimeOffset STARTDATE { get; set; }
        public DateTimeOffset? ENDDATE { get; set; }
        public string COMMENT { get; set; }
        public string JOB_NUMBER { get; set; }

        public virtual REQUEST REQUEST { get; set; }
        public virtual LOCATION SUPPLIER { get; set; }
        public virtual USER USER { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }

        public object Clone()
        {
            var job = new REQUEST_JOB(this);

            var s = JsonConvert.SerializeObject(job,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var r = JsonConvert.DeserializeObject<REQUEST_JOB>(s);
            return r;
        }

        public REQUEST_JOB() { }

        public REQUEST_JOB(REQUEST_JOB record)
        {
            this.COMMENT = record.COMMENT;
            this.ENDDATE = record.ENDDATE;
            this.GUID_RECORD = record.GUID_RECORD;
            this.JOB_NUMBER = record.JOB_NUMBER;
            this.REQUEST_GUID = record.REQUEST_GUID;
            this.STARTDATE = record.STARTDATE;
            this.SUPPLIER_GUID = record.SUPPLIER_GUID;
            this.USER_GUID = record.USER_GUID;

            if (record.REQUEST != null)
            {
                this.REQUEST = new REQUEST(record.REQUEST);
            }

            if (record.SUPPLIER != null)
            {
                this.SUPPLIER = new LOCATION(record.SUPPLIER);
            }

            if (record.USER != null)
            {
                this.USER = new USER(record.USER);
            }
        }
    }

    /// <summary>
    /// Наблюдатели (аудиторы)
    /// </summary>
    [Table("WH_AUDITORS")]
    public class AUDITOR : DbClass
    {
        public Guid REFERENCE_GUID { get; set; }

        [ForeignKey("USER")]
        public Guid USER_GUID { get; set; }

        public DateTimeOffset STARTDATE { get; set; }

        public virtual USER USER { get; set; }

        [ForeignKey("REFERENCE_GUID")]
        public virtual ICollection<REMINDER_MESSAGES> MESSAGES { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }
    }

    [Table("WH_CONNECTION_TYPES")]
    public class CONNECTION_TYPE : DbClass
    {
        public string CONNECTION_TYPE_NAME { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }
    }

    [Table("WH_CONNECTIONS")]
    public class CONNECTION : DbClass
    {
        [ForeignKey("CONNECTION_TYPE")]
        public Guid CONNECTION_TYPE_GUID { get; set; }
        public Guid LEFT_GUID { get; set; }
        public Guid RIGHT_GUID { get; set; }

        public CONNECTION_TYPE CONNECTION_TYPE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }
    }

    [Table("WH_REQUEST_NOTIFICATION_LIST")]
    public class REQUEST_NOTIFICATION_ITEM : DbClass
    {
        public Guid CATEGORY_GUID { get; set; }

        [ForeignKey("USER")]
        public Guid USER_GUID { get; set; }

        public Guid? PU_GUID { get; set; }
        public Guid? COMPANY_GUID { get; set; }
        public Guid? STORE_GUID { get; set; }
        public Guid? BRAND_GUID { get; set; }
        public Guid? LOCATION_TYPE_GUID { get; set; }
        public bool USE_EMAIL { get; set; }
        public bool USE_PHONE { get; set; }


        public virtual USER USER { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }
    }

    [Table("WH_INCIDENT_NOTIFICATION_LIST")]
    public class INCIDENT_NOTIFICATION_ITEM : DbClass
    {
        public Guid CATEGORY_GUID { get; set; }

        public Guid USER_GUID { get; set; }

        public Guid? PU_GUID { get; set; }
        public Guid? COMPANY_GUID { get; set; }
        public Guid? STORE_GUID { get; set; }
        public Guid? BRAND_GUID { get; set; }
        public Guid? LOCATION_TYPE_GUID { get; set; }
        public bool USE_EMAIL { get; set; }
        public bool USE_PHONE { get; set; }

        public bool USE_WHEN_OPEN { get; set; }
        public bool USE_WHEN_CLOSE { get; set; }

        [ForeignKey("USER_GUID")]
        public virtual USER USER { get; set; }

        [ForeignKey("PU_GUID")]
        public LOCATION PU { get; set; }

        [ForeignKey("COMPANY_GUID")]
        public LOCATION COMPANY { get; set; }

        [ForeignKey("STORE_GUID")]
        public LOCATION STORE { get; set; }

        [ForeignKey("CATEGORY_GUID")]
        public LOCATION CATEGORY { get; set; }

        [ForeignKey("BRAND_GUID")]
        public BRAND BRAND { get; set; }

        [ForeignKey("LOCATION_TYPE_GUID")]
        public LOCATION_TYPE LOCATION_TYPE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }


        public object Clone()
        {
            var s = JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var r = JsonConvert.DeserializeObject<INCIDENT_NOTIFICATION_ITEM>(s);
            return r;
        }
    }

    [Table("WH_INCIDENTS")]
    public class INCIDENT : DbClass
    {
        [Column("INCIDENT_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int INCIDENT_ID { get; set; }

        [Column("REQUEST_GUID"), ForeignKey("REQUEST")]
        public Guid? REQUEST_GUID { get; set; }

        [Column("PU_GUID"), ForeignKey("PU")]
        public Guid PU_GUID { get; set; }

        [Column("COMPANY_GUID"), ForeignKey("COMPANY")]
        public Guid COMPANY_GUID { get; set; }

        [Column("STORE_GUID"), ForeignKey("STORE")]
        public Guid STORE_GUID { get; set; }

        [ForeignKey("OPERATOR")]
        public Guid? OPERATOR_GUID { get; set; }

        [ForeignKey("CLOSE_OPERATOR")]
        public Guid? CLOSE_OPERATOR_GUID { get; set; }

        //[Column("PERSON_CATEGORY_GUID"), ForeignKey("PERSON_CATEGORY")]
        //public Guid? PERSON_CATEGORY_GUID { get; set; }

        [Column("CATEGORY_GUID"), ForeignKey("CATEGORY")]
        public Guid? CATEGORY_GUID { get; set; }

        [Column("DAMAGE_GUID"), ForeignKey("DAMAGE")]
        public Guid DAMAGE_GUID { get; set; }

        [Column("RESPONSIBLE_UNIT_GUID"), ForeignKey("RESPONSIBLE_UNIT")]
        public Guid? RESPONSIBLE_UNIT_GUID { get; set; }

        [Column("USER_NAME")]
        public string USER_NAME { get; set; }

        [Column("OPERATOR_NAME")]
        public string OPERATOR_NAME { get; set; }

        [Column("CLOSE_OPERATOR_NAME")]
        public string CLOSE_OPERATOR_NAME { get; set; }

        [Column("DESCRIPTION")]
        public string DESCRIPTION { get; set; }

        [Column("INCIDENT_DATE")]
        public DateTimeOffset INCIDENT_DATE { get; set; }

        [Column("COMPLETE_DATE")]
        public DateTimeOffset? COMPLETE_DATE { get; set; }

        [Column("IS_PLAN")]
        public bool IS_PLAN { get; set; }

        public virtual REQUEST REQUEST { get; set; }

        public virtual LOCATION PU { get; set; }
        public virtual LOCATION COMPANY { get; set; }
        public virtual LOCATION STORE { get; set; }
        public virtual LOCATION RESPONSIBLE_UNIT { get; set; }
        //public virtual LOCATION PERSON_CATEGORY { get; set; }
        public virtual LOCATION CATEGORY { get; set; }
        public virtual LOCATION DAMAGE { get; set; }
        public virtual USER OPERATOR { get; set; }
        public virtual USER CLOSE_OPERATOR { get; set; }

        [ForeignKey("REFERENCE_GUID")]
        public virtual ICollection<AUDITOR> AUDITORS { get; set; }

        [Column("DATECREATE")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [Column("DATEUPDATE")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [Column("USERCREATE")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [Column("USERUPDATE")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }

        public INCIDENT Clone()
        {
            var s = JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var r = JsonConvert.DeserializeObject<INCIDENT>(s);
            return r;
        }
    }

    [Table("WH_CACHE")]
    public class CACHE : DbClass
    {
        public Guid REFERENCE_GUID { get; set; }

        public DateTime? REFERENCE_DATE { get; set; }
        public string REFERENCE_TYPE { get; set; }
        public string JSON { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }
    }

    [Table("WH_AUDIT")]
    public class AUDIT : DbClass
    {
        public Guid REFERENCE_GUID { get; set; }
        public Guid USER_GUID { get; set; }
        public DateTimeOffset AUDIT_DATE { get; set; }
        public string REFERENCE_TYPE_NAME { get; set; }
        public string ACTION_NAME { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }

        public enum ACTIONS
        {
            READ, CREATE, UPDATE, DELETE
        }
    }

    [Table("UV_REMINDER_MESSAGES")]
    public class REMINDER_MESSAGES : DbClass
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Message { get; set; }

        //[Key]
        //public Guid? GUID_RECORD { get; set; }
        public byte ID_MsgType { get; set; }
        public string Recipient { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime? DateComplete { get; set; }
        //public DateTime? DateCreate { get; set; }
        //public DateTime? DateUpdate { get; set; }


        public byte RetryCount { get; set; }
        public string ErrorMessage { get; set; }

        [Column("BeeLineNumber")]
        public string ProviderMessageNumber { get; set; }

        public Guid? REFERENCE_GUID { get; set; }
        public Guid? SESSION_GUID { get; set; }
        public Guid? PU_GUID { get; set; }
        public Guid? COMPANY_GUID { get; set; }

        public string   ProviderName { get; set; }
        public string   SenderName { get ; set; }
        public string   CarbonCopy { get ; set; }
        public string   BlindCarbonCopy { get ; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool IS_COMPLETE { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateCreate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateUpdate { get; set; }


        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTimeOffset DateCreate { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTimeOffset DateUpdate { get; set; }

        [ForeignKey("OWNER_GUID")]
        public virtual ICollection<REMINDER_MESSAGE_ATTACHMENTS> Attachments { get; set; }
    }

    [Table("UV_REMINDER_MESSAGE_ATTACHMENTS")]
    public class REMINDER_MESSAGE_ATTACHMENTS : DbClass
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int ID_Record { get; set; }
        //public int ID_Message { get; set; }
        public string Path { get; set; }
        public bool IsDeleteAfterSending { get; set; }
        public bool IsMoveAfterSending { get; set; }
        public string PathToMoveAferSending { get; set; }

        public Guid? OWNER_GUID { get; set; }
        public byte[] Stream { get; set; }
        public string FileName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateCreate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateUpdate { get; set; }

        [ForeignKey("OWNER_GUID")]
        public REMINDER_MESSAGES Message { get; set; }
    }

    [Table("WH_TEMPLATES")]
    public class TEMPLATE : DbClass
    {
        public string NAME { get; set; }
        public string SYSTEM_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string TEXT { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }
    }

    [Table("WH_USER_CONTACTS")]
    public class USER_CONTACT : DbClass
    {
        public Guid USER_GUID { get; set; }
        public string ADDRESS { get; set; }
        public int TYPE_CODE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATECREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset DATEUPDATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERCREATE { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string USERUPDATE { get; set; }
    }
}


