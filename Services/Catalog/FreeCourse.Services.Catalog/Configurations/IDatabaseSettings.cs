namespace FreeCourse.Services.Catalog.Configurations
{
    internal interface IDatabaseSettings
    {
        public string CourseCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
