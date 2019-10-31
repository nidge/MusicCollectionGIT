namespace MusicCollection2017.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnknownChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recording", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recording", "Title", c => c.String());
        }
    }
}
