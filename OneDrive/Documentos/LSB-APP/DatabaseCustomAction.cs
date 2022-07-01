using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSB.App.Database;
using LSB.App.Forms;
using LSB.App.Log;
using Preactor;
using Preactor.Interop.PreactorObject;

namespace LSB.App
{
    [Guid("e9dd58d2-b919-47fb-a290-d9953f7238f8")]
    [ComVisible(true)]
    public interface IDatabaseCustomAction
    {
        int ExecuteStoredProcedure(ref PreactorObj preactorComObject, ref object pespComObject, string pStoredProcedure, string pTitulo, int pTimeOut);
        int DisplayMessage(ref PreactorObj preactorComObject, ref object pespComObject, string pTitulo, string pMessage);
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("1044233d-2d6e-41a7-ad5e-305e042b8eb7")]
    public class DatabaseCustomAction : IDatabaseCustomAction
    {
        private DatabaseUtil _databaseUtil;
        private WaitWindow _loadingWindow;
        private bool isRunning;
        public DatabaseCustomAction()
        {
            LogUtil.InitializeLogs();
        }

        public int ExecuteStoredProcedure(ref PreactorObj preactorComObject, ref object pespComObject, string pStoredProcedure, string pTitulo, int pTimeOut)
        {
            isRunning = true;

            IPreactor preactor = PreactorFactory.CreatePreactorObject(preactorComObject);
            
            _databaseUtil = new DatabaseUtil(preactor);
            try
            {
                _databaseUtil.OnExecuteStoredProcedureComplete += DatabaseUtil_OnExecuteStoredProcedureComplete;
                _databaseUtil.OnExecuteStoredProcedureError += _databaseUtil_OnExecuteStoredProcedureError;
                _loadingWindow = new WaitWindow() { Text = pTitulo };
                _loadingWindow.HandleCreated += (a, b) => { };
                _loadingWindow.HandleDestroyed += (a, b) => { };
                _loadingWindow.RunInNewThread(false);
                _databaseUtil.ExecuteStoredProcedure(pStoredProcedure, pTimeOut);
                
            }
            catch
            {
                MessageBox.Show($"Erro ao executar stored procedure {pStoredProcedure}. Para maiores detalhes consulte o arquivo de logs.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _loadingWindow.Invoke(new Action(() =>
                {
                    _loadingWindow.Close();
                }));
            }
            return 0;
        }

        private void _databaseUtil_OnExecuteStoredProcedureError(object sender, string e)
        {
            _loadingWindow.Invoke(new Action(() =>
            {
                _loadingWindow.Close();
                MessageBox.Show($"Erro ao executar stored procedure {e}. Para maiores detalhes consulte o arquivo de logs.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
            isRunning = false;
        }

        private void DatabaseUtil_OnExecuteStoredProcedureComplete(object sender, string e)
        {
            _loadingWindow.Invoke(new Action(() =>
            {
                _loadingWindow.Close();
            }));
            isRunning = false;
        }

        public int DisplayMessage(ref PreactorObj preactorComObject, ref object pespComObject, string pTitulo, string pMessage)
        {
            MessageBox.Show(pMessage, pTitulo);
            return 0;
        }
    }
}
