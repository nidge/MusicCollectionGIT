namespace MusicCollection2017.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUrlToRecordings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recording", "Url", c => c.String());
            AlterColumn("dbo.Artist", "Title", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Artist", "Title", c => c.String());
            DropColumn("dbo.Recording", "Url");
        }
    }
}
