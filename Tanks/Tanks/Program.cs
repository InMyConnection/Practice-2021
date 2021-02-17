using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                if (args.Length > 0)
                {
                    int width = int.Parse(args[0]);//������� �������� ����(������)
                    int height = int.Parse(args[1]);//������� �������� ����(������)
                    int countOfTanks = int.Parse(args[2]);//���������� ������ �� ����
                    int countOfApples = int.Parse(args[3]);//���������� ����� �� ����
                    int speed = int.Parse(args[4]);//�������� ������������ ��������(�������� ��� ���� �������� �����)
                    Application.Run(new Form1(width, height, countOfTanks, countOfApples, speed));
                }
                else
                    Application.Run(new Form1());
            }
            catch
            {
                MessageBox.Show("���������� � �������� ���������� ������� ������ ������������ ��������� ��������� " +
                    "� �������� �������: ������ �������� ����, ������ �������� ����, ���������� ������ �� ����, " +
                    "���������� ����� �� ����, �������� ������������ ��������(�������� ��� ���� �������� �����).", 
                    "Battle city");
            } 
        }
    }
}
