namespace FreeCourse.Services.Catalog.Configurations
{
    public class DatabaseSettings : IDatabaseSettings //Bu propertyleri startup tarafında set edeceğiz.
        //appsettings.json dosyasını okuyacağız
    {
        public string CourseCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set ; }
        public string DatabaseName { get; set; }
    }
}
