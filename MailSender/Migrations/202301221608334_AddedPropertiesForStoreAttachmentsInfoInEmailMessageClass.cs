namespace MailSender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPropertiesForStoreAttachmentsInfoInEmailMessageClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailMessages", "AttachmentsDirectoryPath", c => c.String());
            AddColumn("dbo.EmailMessages", "AttachmentsFileNames", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmailMessages", "AttachmentsFileNames");
            DropColumn("dbo.EmailMessages", "AttachmentsDirectoryPath");
        }
    }
}
