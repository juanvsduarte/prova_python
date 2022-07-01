using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSB.App.Database.Model
{
    public class QuebraOPEstadosCalendarios
    {
        //        public const string QUERY = @"
        //SELECT [datasetid] as DatasetId
        //      ,[Id_recurso] as IdRecurso
        //      ,[recurso] as Recurso
        //      ,[Id_calendario] as IdCalendario
        //      ,[calendario] as Calendario
        //      ,[estado] as Estado
        //      ,[data_referencia] as DataReferencia
        //      ,[inicio_estado] as InicioEstado
        //      ,[fim_estado] as FimEstado
        //  FROM[dbo].[LSB_Quebra_OP_Estados_Calendarios]";
        public const string QUERY = @"
SELECT [datasetid] as datasetid
      ,[Id_recurso] as IdRecurso
      ,[recurso] as Recurso
      ,[Id_calendario] as IdCalendario
      ,[calendario] as Calendario
      ,[estado] as Estado
      ,[data_referencia] as DataReferencia
      ,[inicio_estado] as InicioEstado
      ,[fim_estado] as FimEstado
  FROM[dbo].[LSB_APP_Quebra_OP_Estados_Calendarios]";
        [Dapper.Column("datasetid")]
        public int DatasetId { get; set; }
        [Dapper.Column("IdRecurso")]
        public int IdRecurso { get; set; }
        [Dapper.Column("Recurso")]
        public string Recurso { get; set; }
        [Dapper.Column("IdCalendario")]
        public int IdCalendario { get; set; }
        [Dapper.Column("Calendario")]
        public string Calendario { get; set; }
        [Dapper.Column("Estado")]
        public string Estado { get; set; }
        [Dapper.Column("DataReferencia")]
        public DateTime DataReferencia { get; set; }
        [Dapper.Column("InicioEstado")]
        public DateTime InicioEstado { get; set; }
        [Dapper.Column("FimEstado")]
        public DateTime FimEstado { get; set; }
    }
}
