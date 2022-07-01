using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Dapper;
using LSB.App.Database;
using LSB.App.Database.Model;
using LSB.App.Forms;
using LSB.App.Log;
using Preactor;
using Preactor.Interop.PreactorObject;

namespace LSB.App
{
    [Guid("4B7822AB-D502-470B-9B18-3DEB1B3A334C")]
    [ComVisible(true)]
    public interface ICalendarCustomAction
    {
        int BreakBars(ref PreactorObj preactorComObject, ref Preactor.Interop.EventScripts.Core pespComObject);
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("DFA118FF-5963-4E06-A6EB-8BE8BF651894")]
    public class CalendarCustomAction : ICalendarCustomAction
    {
        public CalendarCustomAction()
        {
            LogUtil.InitializeLogs();
        }
        private DatabaseUtil<QuebraOPQuantidadesPeriodo> _databaseUtil;
        private WaitWindow _loadingWindow;
        private IPreactor _preactor;
        private IPlanningBoard _planningBoard;
        private bool isRunning;
        public int BreakBars(ref PreactorObj preactorComObject, ref Preactor.Interop.EventScripts.Core pespComObject)
        {
            try
            {
                isRunning = true;
                _preactor = PreactorFactory.CreatePreactorObject(preactorComObject);
                _planningBoard = _preactor.PlanningBoard;

                _loadingWindow = new WaitWindow() { Text = "Executando quebra de barras" };
                _loadingWindow.HandleCreated += (a, b) => { };
                _loadingWindow.HandleDestroyed += (a, b) => { };
                _loadingWindow.RunInNewThread(false);

                _databaseUtil = new DatabaseUtil<QuebraOPQuantidadesPeriodo>(_preactor);
                _databaseUtil.OnExecuteQueryComplete += _databaseUtil_OnExecuteQueryComplete;
                _databaseUtil.OnExecuteQueryError += _databaseUtil_OnExecuteQueryError;

                var calendarState = _databaseUtil.ExecuteQuery(QuebraOPQuantidadesPeriodo.QUERY);
                ProcessQuantities(calendarState);
            }
            catch(Exception ex)
            {
                _loadingWindow.Invoke(new Action(() =>
                {
                    _loadingWindow.Hide();
                }));
                MessageBox.Show("Não foi possível completar a operação. Consulte os logs.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }

        private void _databaseUtil_OnExecuteQueryError(object sender, string e)
        {
            _loadingWindow.Invoke(new Action(() =>
            {
                _loadingWindow.Hide();
                MessageBox.Show(e, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
            isRunning = false;
        }

        private void _databaseUtil_OnExecuteQueryComplete(object sender, System.Collections.Generic.IEnumerable<QuebraOPQuantidadesPeriodo> calendarState)
        {
            ProcessQuantities(calendarState);
        }

        private void ProcessQuantities(System.Collections.Generic.IEnumerable<QuebraOPQuantidadesPeriodo> calendarState)
        {
            int count = 0;
            var databaseUtil = new DatabaseUtil<QuebraOPQuantidadesPeriodo>(_preactor);
            foreach (var item in calendarState.OrderBy(x => x.Id))
            {
                try
                {
                    var resourceNumber = _planningBoard.GetResourceNumber(item.Recurso.ToString());
                    var quantity = _planningBoard.GetProcessedQuantity(resourceNumber, item.Operacao, item.Inicio, item.Fim);
                    count++;
                    item.Quantidade = quantity;
                    databaseUtil.Save(item);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex.ToString());
                }
            }
            _loadingWindow.Invoke(new Action(() =>
            {
                _loadingWindow.Hide();
            }));
            isRunning = false;
        }
    }
}
