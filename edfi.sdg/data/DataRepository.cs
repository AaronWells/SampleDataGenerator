namespace EdFi.SampleDataGenerator.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using EdFi.SampleDataGenerator.Models;

    public class DataRepository
    {
        public void Save(IComplexObjectType obj)
        {
            using (var model = new DataModel())
            {
                model.Database.ExecuteSqlCommand("dbo.upsertComplexObject @identifier, @className, @xml",
                    new SqlParameter("@identifier", obj.id),
                    new SqlParameter("@className", obj.GetType().ToString()),
                    new SqlParameter("@xml", obj.ToXml())
                );
            }
        }

        public void Clear()
        {
            using (var model = new DataModel())
            {
                model.Database.ExecuteSqlCommand("truncate table dbo.ComplexObject");
            }
        }

        internal class GetByIdDTO
        {
            public string ClassName { get; set; }

            public string Xml { get; set; }
        }

        public IComplexObjectType GetById(string identifier)
        {
            using (var model = new DataModel())
            {
                var query = model.Database.SqlQuery<GetByIdDTO>(
                    "select ClassName, Xml from dbo.ComplexObject where identifier = @identifier", new SqlParameter("@identifier", identifier));

                var result = query.First();
                return result != null ? ComplexObjectTypeExtensions.FromXml(result.ClassName, result.Xml) : null;
            }
        }

        public IComplexObjectType[] GetByClassName(string className)
        {
            using (var model = new DataModel())
            {
                var query = model.Database.SqlQuery<string>(
                    "select Xml from dbo.ComplexObject where ClassName = @identifier", new SqlParameter("@className", className));

                var result = query.Select(x => ComplexObjectTypeExtensions.FromXml(className, x)).ToArray();
                return result;
            }
        }

        public ComplexObjectType[] GetByExample(dynamic obj)
        {
            throw new NotImplementedException();
        }

        public string GetNextValue(string statTableName, IEnumerable<string> attributes)
        {
            using (var model = new DataModel())
            {
                var dataTable = ToDataTable(attributes);

                var query = model.Database.SqlQuery<string>("exec dbo.GetNextValue @StatTableName, @AttributeFilters",
                    new SqlParameter("@StatTableName", statTableName),
                    new SqlParameter("@AttributeFilters", SqlDbType.Structured) { Value = dataTable, TypeName = "AttributeList" }
                    );

                return query.FirstOrDefault();
            }
        }

        private static DataTable ToDataTable(IEnumerable<string> items)
        {
            var dataTable = new DataTable("Attributes");

            dataTable.Columns.Add("Attribute");

            foreach (var item in items)
            {
                dataTable.Rows.Add(item);
            }

            return dataTable;
        }
    }
}
