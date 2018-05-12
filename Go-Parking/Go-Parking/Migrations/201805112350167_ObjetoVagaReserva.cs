namespace Go_Parking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObjetoVagaReserva : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Reserva", name: "Vaga_Id", newName: "Vagas_Id");
            RenameColumn(table: "dbo.Reserva", name: "Veiculo_Id", newName: "Veiculos_Id");
            RenameIndex(table: "dbo.Reserva", name: "IX_Vaga_Id", newName: "IX_Vagas_Id");
            RenameIndex(table: "dbo.Reserva", name: "IX_Veiculo_Id", newName: "IX_Veiculos_Id");
            AlterColumn("dbo.Reserva", "Entrada", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Reserva", "Saida", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reserva", "Saida", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Reserva", "Entrada", c => c.DateTimeOffset(precision: 7));
            RenameIndex(table: "dbo.Reserva", name: "IX_Veiculos_Id", newName: "IX_Veiculo_Id");
            RenameIndex(table: "dbo.Reserva", name: "IX_Vagas_Id", newName: "IX_Vaga_Id");
            RenameColumn(table: "dbo.Reserva", name: "Veiculos_Id", newName: "Veiculo_Id");
            RenameColumn(table: "dbo.Reserva", name: "Vagas_Id", newName: "Vaga_Id");
        }
    }
}
