using System;

namespace Nishan.Data
{
    /// <summary>
    /// Type of SQL command
    /// </summary>
    public enum DBCmdType
    {
        /// <summary>
        /// Command Type is unknown
        /// </summary>
        DbCmdUnknown,
        /// <summary>
        /// A select statement
        /// </summary>
        DbCmdSelect,
        /// <summary>
        /// An update statement
        /// </summary>
        DbCmdUpdate,
        /// <summary>
        /// An insert statement
        /// </summary>
        DbCmdInsert,
        /// <summary>
        /// A delete statement
        /// </summary>
        DbCmdDelete,
        /// <summary>
        /// Command is a Stored Procedure call
        /// </summary>
        DbCmdStoredProc
    }

    /// <summary>
    /// DBMS Type
    /// </summary>
    public enum DBMSType
    {
        /// <summary>
        /// DBMS is unknown/not set
        /// </summary>
        DbUnknown = 1,
        /// <summary>
        /// Microsoft Access Database
        /// </summary>
        DbMsAccess,
        /// <summary>
        /// Microsoft SQL Server Database
        /// </summary>
        DbMsSQLSrv,
        /// <summary>
        /// Oracle Database
        /// </summary>
        DbOracle,
        /// <summary>
        /// IBM DB2 database
        /// </summary>
        DbIBMDb2,

        DbMySQL
    };


    /// <summary>
    /// Encapsulates all the information required to execute a database command 
    /// </summary>
    public struct SQLUnit
    {
        /// <summary>
        /// Type of Command
        /// </summary>
        public DBCmdType CommandType;
        /// <summary>
        /// SQL Command
        /// </summary>
        public String SQL;
        /// <summary>
        /// Parameterised Constructor
        /// </summary>
        /// <param name="Type">Type of command</param>
        /// <param name="SQL">SQL Statement</param>
        public SQLUnit(DBCmdType Type, String SQL)
        {
            CommandType = Type;
            this.SQL = SQL;
        }
    }
    /// <summary>
    /// Type of data columns 
    /// </summary>
    public enum DBColType
    {
        /// <summary>
        /// Column is a normal data column
        /// </summary>
        ColNormal,
        /// <summary>
        /// Column is an identity column with database side auto-increment
        /// </summary>
        ColIdentityMax,
        /// <summary>
        /// Column is a database column with fron-end side auto increment
        /// </summary>
        ColMaxNumber,
        /// <summary>
        /// Column contains system datetime at the time of insertions updations
        /// </summary>
        ColLMDT,
        ColVisitDT,
        ColCreditCardExpDT,
        /// <summary>
        /// Column contains Creation date time of Object
        /// </summary>
        ColEntryDT,
        ColUserLoginId,
        ColMachine,
        ColSiteCode,
        ColCompCode,
        ColEntryUser,
        ColEntryMachine,
        ColFinancialYear,
        ColAutoPosted

    };
    /// <summary>
    /// A unit of Column and a validation object applied on it 
    /// </summary>
    //public struct DbValidationUnit
    //{
    //    /// <summary>
    //    /// Reference of validation object
    //    /// </summary>
    //    internal BLVldBase ValidationObject;
    //    /// <summary>
    //    /// Column object to validate
    //    /// </summary>
    //    internal BLDBCol CurrentColumn;
    //}
    ///// <summary>
    ///// Type of data source
    ///// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// Source is unknown
        /// </summary>
        SRCUnknown = 0,
        /// <summary>
        /// Source is a database table
        /// </summary>
        SRCTable,
        /// <summary>
        /// Source is a SQL query
        /// </summary>
        SRCQuery,
        /// <summary>
        /// Source is a stored procedure
        /// </summary>
        SRCStoredProc,
        /// <summary>
        /// Source is a database view
        /// </summary>
        SRCView

    };
    /// <summary>
    /// Nature of the data source DBMS object
    /// </summary>
    public enum DataTargetType
    {
        /// <summary>
        /// Target type is unknown
        /// </summary>
        SRCUnknown = 0,
        /// <summary>
        /// Target is a database updatble view
        /// </summary>
        SRCQuery,
        /// <summary>
        /// Target is a table name
        /// </summary>
        SRCTable,
        /// <summary>
        /// Target is a Stored Procedure
        /// </summary>
        SRCStoredProc
    };
}