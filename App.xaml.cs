﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace KeyboardMaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (m_globalHook == null)//Оформляем событие по вытягиванию нажатых клавиш
            {
                m_globalHook = Hook.GlobalEvents();
                m_globalHook.KeyDown += GlobalHookKeyDown;
            }
        }
        private IKeyboardMouseEvents m_globalHook;
        private void GlobalHookKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)//Обработчик собыьтия по вытягиванию нажатых клавиш
        {
            //Действие при вызове события
            // если нажали клавишу
        }


        private void GlobalHookKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //Действие при вызове события
            // если зажали клавишу
        }

    }
}
