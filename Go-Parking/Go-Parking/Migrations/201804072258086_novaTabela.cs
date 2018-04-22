namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class novaTabela : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RoleViewModel");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoleViewModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
