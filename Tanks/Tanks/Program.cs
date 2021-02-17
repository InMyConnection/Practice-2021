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
                    int width = int.Parse(args[0]);//Размеры игрового поля(ширина)
                    int height = int.Parse(args[1]);//Размеры игрового поля(высота)
                    int countOfTanks = int.Parse(args[2]);//Количество танков на поле
                    int countOfApples = int.Parse(args[3]);//Количество яблок на поле
                    int speed = int.Parse(args[4]);//Скорость передвижения объектов(задается для всех объектов сразу)
                    Application.Run(new Form1(width, height, countOfTanks, countOfApples, speed));
                }
                else
                    Application.Run(new Form1());
            }
            catch
            {
                MessageBox.Show("Приложению в качестве параметров запуска должны передаваться следующие параметры " +
                    "в числовом формате: ширина игрового поля, высота игрового поля, количество танков на поле, " +
                    "количество яблок на поле, скорость передвижения объектов(задается для всех объектов сразу).", 
                    "Battle city");
            } 
        }
    }
}
