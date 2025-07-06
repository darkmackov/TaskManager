using FluentMigrator;

namespace TaskManager.Database.Migrations
{
    [Migration(0)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("TaskItems")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Title").AsString(128).NotNullable()
                .WithColumn("Description").AsString(4096).NotNullable()
                .WithColumn("State").AsInt16().NotNullable().WithDefaultValue(0)
                .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("DueDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
