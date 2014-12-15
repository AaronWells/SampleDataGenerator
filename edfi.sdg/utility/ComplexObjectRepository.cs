namespace edfi.sdg.utility
{
    using System;
    using System.Linq;

    using edfi.sdg.data;
    using edfi.sdg.models;

    public class ComplexObjectRepository
    {
        public void Save(ComplexObjectType obj)
        {
            using (var model = new DataModel())
            {
                var className = obj.GetType().ToString();

                var complexObjectClass = model.ComplexObjectClasses.FirstOrDefault(x => x.Name == className)
                                         ?? new edfi.sdg.data.ComplexObjectClass { Name = className };

                var complexObject = complexObjectClass.ComplexObjects.FirstOrDefault(x => x.Id == obj.id);

                if (complexObject == null)
                {
                    complexObjectClass.ComplexObjects.Add(
                        new ComplexObject() { Id = IdentifierGenerator.Create(), Xml = obj.ToXml() });
                }
                else
                {
                    complexObject.Xml = obj.ToXml();
                }
                model.SaveChanges();
            }
        }

        public ComplexObjectType GetbyId(string identifier)
        {
            using (var model = new DataModel())
            {
                var result = model.ComplexObjects.FirstOrDefault(x => x.Id == identifier);
                return result != null ? ComplexObjectType.FromXml(result.Xml) : null;
            }
        }

        public ComplexObject[] GetByExample(dynamic obj)
        {
            throw new NotImplementedException();
        }
    }
}
