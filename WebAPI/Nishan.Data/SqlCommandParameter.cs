namespace Nishan.Data
{
    public class SqlCommandParameter
    {
        public SqlCommandParameter()
        {
        }

        public SqlCommandParameter(string name, object value, SqlParameterDirection parameterDirection = SqlParameterDirection.In)
        {
            this.Name = name;
            this.ParameterDirection = parameterDirection;
            this.Value = value;

        }
        public string Name { get; set; }

        public SqlParameterDirection ParameterDirection { get; set; }
        public object Value { get; set; }


    }

}


