using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSB.App.Database.Model
{
    [Table("LSB_APP_Quebra_OP_Quantidades_Periodo")]
    public class QuebraOPQuantidadesPeriodo
    {
        public const string QUERY = @"
SELECT [Id]
      ,[Datasetid]
      ,[Operacao]
      ,[Recurso]
      ,[Estado_Calendario] as EstadoCalendario
      ,[Inicio]
      ,[Fim]
      ,[Quantidade]
  FROM [dbo].[LSB_APP_Quebra_OP_Quantidades_Periodo]";
        [Key]
        public int Id { get; set; }
        [Column("Datasetid")]
        public int Datasetid { get; set; }
        [Column("Operacao")]
        public int Operacao { get; set; }
        [Column("Recurso")]
        public string Recurso { get; set; }
        [Column("Estado_Calendario")]
        public string EstadoCalendario { get; set; }
        [Column("Inicio")]
        public DateTime Inicio { get; set; }
        [Column("Fim")]
        public DateTime Fim { get; set; }
        [Column("Quantidade")]
        public Double Quantidade { get; set; }
    }
}
