﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//non default dependencies
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Diagnostics;

namespace Cartographer_Launcher
{
    public partial class launcher_form : Form
    {
        #region Dependencies
        private string register = "http://www.cartographer.h2pc.org/";
        private bool login_check, register_check, settings_check, update_check, sPanel_check, aPanel_check;
        public const int WM_NCLBUTTONDOWN = 0xA1, HT_CAPTION = 0x2, WS_MINIMIZEBOX = 0x20000, CS_DBLCLKS = 0x8;

        public delegate void MouseMovedEvent();

        FontFamily hgb, cil;
        Font handel_20b, handel_18b, handel_28b, conduit_14, conduit_12, conduit_12b;

        Color title_text = Color.FromArgb(193, 218, 248); //#C1DAF8
        Color normal_text = Color.FromArgb(104, 164, 227); //#68A4E3
        Color button_text = Color.FromArgb(184, 205, 224); //#B8CDE0
        Color button_hover = Color.FromArgb(33, 63, 132); //#213F84
        Color button_click = Color.FromArgb(46, 83, 166); //#2E53A6
        Color menu_list_text = Color.FromArgb(94, 109, 139); //#5E6D8B
        Color menu_list_hover_text = Color.FromArgb(126, 147, 178); //#7E93B2
        Color menu_list_click_text = Color.FromArgb(178, 211, 246); //#B2D3F6
        Color panel_background = Color.FromArgb(140, 0, 12, 45); //#000C2D
        Color text_box_background = Color.FromArgb(0, 12, 45); //#000C2D

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams pc = base.CreateParams;
                pc.Style |= WS_MINIMIZEBOX;
                pc.ClassStyle |= CS_DBLCLKS;
                pc.ExStyle = pc.ExStyle | 0x2000000; //Prevent flickering
                return pc;
            }
        }

        public class GlobalMouseHandler : IMessageFilter
        {
            private const int WM_MOUSEMOVE = 0x0200;
            public event MouseMovedEvent MouseMovement;

            #region IMessageFilter Members      
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_MOUSEMOVE)
                    if (MouseMovement != null)
                        MouseMovement();
                return false;
            }
            #endregion
        }
        #endregion

        public launcher_form()
        {
            GlobalMouseHandler gmh = new GlobalMouseHandler();
            gmh.MouseMovement += new MouseMovedEvent(CursorPositionStyleSheet);
            Application.AddMessageFilter(gmh);

            InitializeComponent();

            HandelGothicMedium();
            ConduitITC_light();
            StyleSheet();
            CursorPositionStyleSheet();

            login_check = false;
            register_check = false;
            settings_check = false;
            update_check = false;
            sPanel_check = false;
        }

        private void CursorPositionStyleSheet()
        {
            Point cur_pos = Cursor.Position;
            var rel_pos = this.PointToClient(cur_pos);

            #region Login
            if (rel_pos.X >= login_label.Location.X
                && rel_pos.X <= login_label.Location.X + login_label.Size.Width
                && rel_pos.Y >= login_label.Location.Y
                && rel_pos.Y <= login_label.Location.Y + login_label.Size.Height)
            {
                if (login_check)
                    this.login_label.ForeColor = menu_list_click_text;
                else
                    this.login_label.ForeColor = menu_list_hover_text;
            }
            else
            {
                if (panel_slide.Enabled == true && !settings_check)
                    login_check = true;
                else
                    login_check = false;
                this.login_label.ForeColor = menu_list_text;
            }
            #endregion

            #region Register
            if (rel_pos.X >= register_label.Location.X
                && rel_pos.X <= register_label.Location.X + register_label.Size.Width
                && rel_pos.Y >= register_label.Location.Y
                && rel_pos.Y <= register_label.Location.Y + register_label.Size.Height)
            {
                if (!register_check)
                    this.register_label.ForeColor = menu_list_hover_text;
                else
                    this.register_label.ForeColor = menu_list_click_text;
            }
            else
            {
                this.register_label.ForeColor = menu_list_text;
                register_check = false;
            }
            #endregion

            #region Settings
            if (rel_pos.X >= settings_label.Location.X
                && rel_pos.X <= settings_label.Location.X + settings_label.Size.Width
                && rel_pos.Y >= settings_label.Location.Y
                && rel_pos.Y <= settings_label.Location.Y + settings_label.Size.Height)
            {
                if (settings_check)
                    this.settings_label.ForeColor = menu_list_click_text;
                else
                    this.settings_label.ForeColor = menu_list_hover_text;
            }
            else
            {
                if (panel_slide.Enabled == true && !login_check)
                    settings_check = true;
                else
                    settings_check = false;
                this.settings_label.ForeColor = menu_list_text;
            }
            #endregion

            #region Check Updates
            if (rel_pos.X >= update_label.Location.X
                && rel_pos.X <= update_label.Location.X + update_label.Size.Width
                && rel_pos.Y >= update_label.Location.Y
                && rel_pos.Y <= update_label.Location.Y + update_label.Size.Height)
            {
                if (!update_check)
                    this.update_label.ForeColor = menu_list_hover_text;
                else
                    this.update_label.ForeColor = menu_list_click_text;
            }
            else
            {
                this.update_label.ForeColor = menu_list_text;
                update_check = false;
            }
            #endregion
        }

        private void HandelGothicMedium()
        {
            byte[] hgm_fontArray = Properties.Resources.handel_gothic_medium;
            int hgm_dataLenght = Cartographer_Launcher.Properties.Resources.handel_gothic_medium.Length;

            IntPtr hgm_ptr = Marshal.AllocCoTaskMem(hgm_dataLenght);
            Marshal.Copy(hgm_fontArray, 0, hgm_ptr, hgm_dataLenght);

            uint hgm_Fonts = 0;
            AddFontMemResourceEx(hgm_ptr, (uint)hgm_fontArray.Length, IntPtr.Zero, ref hgm_Fonts);

            PrivateFontCollection hgm_pfc = new PrivateFontCollection();
            hgm_pfc.AddMemoryFont(hgm_ptr, hgm_dataLenght);

            Marshal.FreeCoTaskMem(hgm_ptr);
            hgb = hgm_pfc.Families[0];
            handel_20b = new Font(hgb, 20, FontStyle.Bold);
            handel_18b = new Font(hgb, 18, FontStyle.Bold);
            handel_28b = new Font(hgb, 28, FontStyle.Bold);
        }

        private void ConduitITC_light()
        {

            byte[] cil_fontArray = Cartographer_Launcher.Properties.Resources.conduit_itc_light;
            int cil_dataLength = Cartographer_Launcher.Properties.Resources.conduit_itc_light.Length;

            IntPtr cil_ptr = Marshal.AllocCoTaskMem(cil_dataLength);
            Marshal.Copy(cil_fontArray, 0, cil_ptr, cil_dataLength);

            uint cil_Fonts = 0;
            AddFontMemResourceEx(cil_ptr, (uint)cil_fontArray.Length, IntPtr.Zero, ref cil_Fonts);

            PrivateFontCollection cil_pfc = new PrivateFontCollection();
            cil_pfc.AddMemoryFont(cil_ptr, cil_dataLength);

            Marshal.FreeCoTaskMem(cil_ptr);
            cil = cil_pfc.Families[0];
            conduit_14 = new Font(cil, 14, FontStyle.Regular);
            conduit_12 = new Font(cil, 12, FontStyle.Regular);
            conduit_12b = new Font(cil, 12, FontStyle.Bold);
        }

        public void StyleSheet()
        {
            //Transparency Fix
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;
            //End Transparency Fix

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.login_panel.BackColor = panel_background;
            this.settings_panel.BackColor = panel_background;
            this.settings_panel.Width = 0;
            this.login_panel.Height = 0;

            #region Designer
            // 
            // minimize_button
            // 
            this.minimize_button.ForeColor = button_text;
            this.minimize_button.FlatAppearance.MouseOverBackColor = button_hover;
            this.minimize_button.FlatAppearance.MouseDownBackColor = button_click;
            // 
            // close_button
            // 
            this.close_button.ForeColor = button_text;
            this.close_button.FlatAppearance.MouseOverBackColor = button_hover;
            this.close_button.FlatAppearance.MouseDownBackColor = button_click;
            // 
            // title_label
            // 
            this.title_label.Font = handel_20b;
            this.title_label.ForeColor = title_text;
            // 
            // login_label
            // 
            this.login_label.Font = handel_18b;
            this.login_label.ForeColor = menu_list_text;
            // 
            // register_label
            // 
            this.register_label.Font = handel_18b;
            this.register_label.ForeColor = menu_list_text;
            // 
            // settings_label
            // 
            this.settings_label.Font = handel_18b;
            this.settings_label.ForeColor = menu_list_text;
            // 
            // update_label
            // 
            this.update_label.Font = handel_18b;
            this.update_label.ForeColor = menu_list_text;
            // 
            // sPanel_title_label
            // 
            this.sPanel_title_label.Font = handel_20b;
            this.sPanel_title_label.ForeColor = title_text;
            // 
            // sPanel_close_label
            // 
            this.sPanel_close_label.Font = handel_28b;
            this.sPanel_close_label.ForeColor = title_text;
            // 
            // sPanel_settings1_label
            // 
            this.sPanel_setting1_label.Font = conduit_14;
            this.sPanel_setting1_label.ForeColor = menu_list_text;
            // 
            // sPanel_settings2_label
            // 
            this.sPanel_setting2_label.Font = conduit_14;
            this.sPanel_setting2_label.ForeColor = menu_list_text;
            // 
            // sPanel_settings3_label
            // 
            this.sPanel_setting3_label.Font = conduit_14;
            this.sPanel_setting3_label.ForeColor = menu_list_text;
            // 
            // sPanel_settings4_label
            // 
            this.sPanel_setting4_label.Font = conduit_14;
            this.sPanel_setting4_label.ForeColor = menu_list_text;
            // 
            // sPanel_settings5_label
            // 
            this.sPanel_setting5_label.Font = conduit_14;
            this.sPanel_setting5_label.ForeColor = menu_list_text;
            // 
            // aPanel_title_label
            // 
            this.aPanel_title_label.Font = conduit_14;
            this.aPanel_title_label.ForeColor = title_text;
            // 
            // aPanel_username_label
            // 
            this.aPanel_username_label.Font = conduit_14;
            this.aPanel_username_label.ForeColor = normal_text;
            // 
            // aPanel_password_label
            // 
            this.aPanel_password_label.Font = conduit_14;
            this.aPanel_password_label.ForeColor = normal_text;
            // 
            // aPanel_remember_label
            // 
            this.aPanel_remember_label.Font = conduit_12;
            this.aPanel_remember_label.ForeColor = normal_text;
            //
            //aPanel_username_textBox
            //
            this.aPanel_username_textBox.BackColor = text_box_background;
            this.aPanel_username_textBox.ForeColor = title_text;
            this.aPanel_username_textBox.Font = conduit_12b;
            //
            //aPanel_password_textBox
            //
            this.aPanel_password_textBox.BackColor = text_box_background;
            this.aPanel_password_textBox.ForeColor = title_text;
            this.aPanel_password_textBox.Font = conduit_12b;
            //
            //aPanel_remember_checkBox
            //
            this.aPanel_remember_checkBox.BackColor = text_box_background;
            this.aPanel_remember_checkBox.ForeColor = title_text;
            this.aPanel_remember_checkBox.FlatAppearance.CheckedBackColor = title_text;
            this.aPanel_remember_checkBox.FlatAppearance.BorderColor = text_box_background;
            #endregion

            this.Refresh();
        }

        private void sPanelSlide()
        {
            login_check = false;
            this.settings_panel.BringToFront();
            if (!sPanel_check)
            {
                if (settings_panel.Width >= 260)
                {
                    sPanel_check = true;
                    this.Refresh();
                    panel_slide.Stop();
                    panel_slide.Enabled = false;
                }
                else
                {
                    settings_panel.Width += 40;
                    this.Refresh();
                }
            }
            else
            {
                if (settings_panel.Width == 0)
                {
                    sPanel_check = false;
                    this.Refresh();
                    panel_slide.Stop();
                    panel_slide.Enabled = false;
                }
                else
                {
                    settings_panel.Width -= 40;
                    this.Refresh();
                }
            }
        }

        private void aPanelSlide()
        {
            settings_check = false;
            this.login_panel.BringToFront();
            if (!aPanel_check)
            {
                if (login_panel.Height >= 138)
                {
                    aPanel_check = true;
                    this.Refresh();
                    panel_slide.Stop();
                    panel_slide.Enabled = false;
                }
                else
                {
                    login_panel.Height += 40;
                    this.Refresh();
                }
            }
            else
            {
                if (login_panel.Height == 0)
                {
                    aPanel_check = false;
                    this.Refresh();
                    panel_slide.Stop();
                    panel_slide.Enabled = false;
                }
                else
                {
                    login_panel.Height -= 40;
                    this.Refresh();
                }
            }
        }

        #region Event Handlers
        private void main_form_MouseDown(object sender, MouseEventArgs e)
        {
            //Handles mouse down event for left click to allow form to be moved
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void close_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimize_button_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void login_label_Click(object sender, EventArgs e)
        {
            login_check = true;
            panel_slide.Enabled = true;
            panel_slide.Start();
        }

        private void register_label_Click(object sender, EventArgs e)
        {
            register_check = true;
            Process.Start(register);
        }

        private void settings_label_Click(object sender, EventArgs e)
        {
            settings_check = true;
            panel_slide.Enabled = true;
            panel_slide.Start();
        }

        private void update_label_Click(object sender, EventArgs e)
        {
            update_check = true;
        }

        private void sPanel_close_label_Click(object sender, EventArgs e)
        {
            settings_check = true;
            panel_slide.Enabled = true;
            panel_slide.Start();
        }

        private void panel_slide_Tick(object sender, EventArgs e)
        {
            if (settings_check)
                sPanelSlide();
            if (login_check)
                aPanelSlide();
        }

        private void login_panel_Click(object sender, EventArgs e)
        {
            this.login_panel.BringToFront();
        }

        private void settings_panel_Click(object sender, EventArgs e)
        {
            this.settings_panel.BringToFront();
        }
        #endregion
    }
}
