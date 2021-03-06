﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

using System.Runtime.InteropServices;

namespace MapasGoogle
{
    public partial class Form1 : Form
    {
        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Creat_Airport(string name, double lat, double lon);


        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Read_Airport(StringBuilder buff, int pos);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Lenght_File();

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Delete_Airport(string name);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Update_Airport(string name, string Nname, double lat, double lon);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool Exist_Airport(string name);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Create_route(string id, string route, double dis);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Read_Route(StringBuilder buff, int pos);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Delete_Route(string id, string rou);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Update_Route(string id, string rou, string rouN, double dis);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Length_R();

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Update_IDroute(string id, string nID);

        [DllImport("C:\\Users\\Mario Flores JR\\Documents\\TrabajosEstructuraDatos1\\TrabajosEstructuraDatos1\\Airport_DLL_BACKEND\\Debug\\Airport_DLL_BACKEND.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Delete_allIDRoutes(string id);

        GMarkerGoogle marker;
        GMapOverlay markerOverlay;
        DataTable dt;

        DataTable dt2;

        int filaSeleccionada = 0;
        int filaSelec = 0;
        double LatInicial = 15.561373;
        double LngInicial = -88.0243623;

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Descripcion", typeof(string)));
            dt.Columns.Add(new DataColumn("Lat", typeof(double)));
            dt.Columns.Add(new DataColumn("Lng", typeof(double)));

            dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("ID", typeof(string)));
            dt2.Columns.Add(new DataColumn("Route", typeof(string)));
            dt2.Columns.Add(new DataColumn("Distance", typeof(string)));

            dataGridView2.DataSource = dt2;

            //dt.Rows.Add("Ubicacion1", LatInicial, LngInicial);
            dataGridView1.DataSource = dt;

            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[2].Visible = false;

            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(LatInicial, LngInicial);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 9;
            gMapControl1.AutoScroll = true;

            //marcador
            markerOverlay = new GMapOverlay("Marcador");
            marker = new GMarkerGoogle(new PointLatLng(LatInicial, LngInicial), GMarkerGoogleType.green);
            markerOverlay.Markers.Add(marker);

            //tooltip
            marker.ToolTipMode = MarkerTooltipMode.Always;
            marker.ToolTipText = string.Format("Ubicacion: \n Latitud: {0} \n Longitud: {1}", LatInicial, LngInicial);
            gMapControl1.Overlays.Add(markerOverlay);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Char delimit = ';';
            for (int i = 0; i < Lenght_File(); i++)
            {
                StringBuilder st = new StringBuilder(1000);
                Read_Airport(st, (i + 1));
                String[] elements = st.ToString().Split(delimit);

                dt.Rows.Add(elements[0], elements[1], elements[2]);

            }

            for (int i = 0; i < Length_R(); i++)
            {
                StringBuilder stb = new StringBuilder(1000);
                Read_Route(stb, (i + 1));
                String[] a = stb.ToString().Split(';');

                dt2.Rows.Add(a[0], a[1], a[2]);
            }

        }

        private void SeleccionarRegistro(object sender, DataGridViewCellEventArgs e)
        {
            filaSeleccionada = e.RowIndex;
            txtDescipcion.Text = dataGridView1.Rows[filaSeleccionada].Cells[0].Value.ToString();
            txtLatitud.Text = dataGridView1.Rows[filaSeleccionada].Cells[1].Value.ToString();
            txtLongitud.Text = dataGridView1.Rows[filaSeleccionada].Cells[2].Value.ToString();
            marker.Position = new PointLatLng(Convert.ToDouble(txtLatitud.Text), Convert.ToDouble(txtLongitud.Text));
            gMapControl1.Position = marker.Position;
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

            txtLatitud.Text = lat.ToString();
            txtLongitud.Text = lng.ToString();
            txtDescipcion.Text = "";

            marker.Position = new PointLatLng(lat, lng);
            marker.ToolTipText = string.Format("Ubicacion: \n Latitud: {0} \n Longitud: {1}", lat, lng);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtDescipcion.Text.Length != 0)
            {
                
                    Creat_Airport(txtDescipcion.Text, Convert.ToDouble(txtLatitud.Text), Convert.ToDouble(txtLongitud.Text));
                    dt.Rows.Add(txtDescipcion.Text, txtLatitud.Text, txtLongitud.Text);
                    txtDescipcion.Text = "";
                    txtLatitud.Text = "";
                    txtLongitud.Text = "";
                
                
            }
            else
            {
                MessageBox.Show("Airport Name Field Empty");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            Delete_Airport(txtDescipcion.Text);
            dataGridView1.Rows.RemoveAt(filaSeleccionada);
            txtDescipcion.Text = "";
            txtLatitud.Text = "";
            txtLongitud.Text = "";

            Char delimit = ';';
            for (int i = 0; i < Lenght_File(); i++)
            {
                StringBuilder st = new StringBuilder(1000);
                Read_Airport(st, (i + 1));
                String[] elements = st.ToString().Split(delimit);

                dt.Rows.Add(elements[0], elements[1], elements[2]);

            }

            for (int i = 0; i < Length_R(); i++)
            {
                StringBuilder st = new StringBuilder(1000);
                Read_Route(st, (i + 1));
                String[] elements = st.ToString().Split(delimit);

                dt.Rows.Add(elements[0], elements[1], elements[2]);
            }
        }



        private void DESTINO_BT_Click(object sender, EventArgs e)
        {
            if (TxTupdate.Text.Length != 0 && txtDescipcion.Text.Length != 0 && txtLatitud.Text.Length != 0 && txtLongitud.Text.Length != 0)
            {
                if (Exist_Airport(txtDescipcion.Text))
                {
                    Update_Airport(txtDescipcion.Text, TxTupdate.Text, Convert.ToDouble(txtLatitud.Text), Convert.ToDouble(txtLongitud.Text));
                    
                    txtDescipcion.Text = "";
                    txtLatitud.Text = "";
                    txtLongitud.Text = "";
                    TxTupdate.Text = "";
                    dt.Rows.Clear();

                    IDtxt.Text = "";
                    RouteTXT.Text = "";
                    DisTXT.Text = "";
                    dt2.Rows.Clear();

                    Char delimit = ';';
                    for (int i = 0; i < Lenght_File(); i++)
                    {
                        StringBuilder st = new StringBuilder(1000);
                        Read_Airport(st, (i + 1));
                        String[] elements = st.ToString().Split(delimit);

                        dt.Rows.Add(elements[0], elements[1], elements[2]);

                    }

                    for(int i = 0; i < Length_R(); i++)
                    {
                        StringBuilder st = new StringBuilder(1000);
                        Read_Route(st, (i + 1));
                        String[] elements = st.ToString().Split(delimit);

                        dt.Rows.Add(elements[0], elements[1], elements[2]);
                    }
                }
                else
                {
                    MessageBox.Show("Check Airport ID");
                }
            }
            else
            {
                MessageBox.Show("Some of the fields are empty");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            filaSelec = e.RowIndex;
            IDtxt.Text = dataGridView2.Rows[filaSelec].Cells[0].Value.ToString();
            RouteTXT.Text = dataGridView2.Rows[filaSelec].Cells[1].Value.ToString();
            DisTXT.Text = dataGridView2.Rows[filaSelec].Cells[2].Value.ToString();
            

        }

        private void AddRoubtn_Click(object sender, EventArgs e)
        {
            if (IDtxt.Text.Length != 0 && DisTXT.Text.Length != 0 && RouteTXT.Text.Length != 0)
            {
               
                if (Create_route(IDtxt.Text, RouteTXT.Text, Convert.ToDouble(DisTXT.Text)) == 1)
                {
                    dt2.Rows.Add(IDtxt.Text, RouteTXT.Text, DisTXT.Text);
                    IDtxt.Text = "";
                    RouteTXT.Text = "";
                    DisTXT.Text = "";
                    Char delimit = ';';
                    for (int i = 0; i < Length_R(); i++)
                    {
                        StringBuilder st = new StringBuilder(1000);
                        Read_Route(st, (i + 1));
                        String[] elements = st.ToString().Split(delimit);

                        dt.Rows.Add(elements[0], elements[1], elements[2]);
                    }

                }
                else
                {
                    MessageBox.Show("Check the airport name");
                }
            }
            else
            {
                MessageBox.Show("Check the fields");
            }
        }

        private void DeleRoubtn_Click(object sender, EventArgs e)
        {
            if (IDtxt.Text.Length != 0 && RouteTXT.Text.Length != 0)
            {
                Delete_Route(IDtxt.Text, RouteTXT.Text);
                IDtxt.Text = "";
                RouteTXT.Text = "";
                DisTXT.Text = "";
                dt2.Rows.RemoveAt(filaSelec);


            }
            else
            {
                MessageBox.Show("Check the fields");
            }
        }

        private void Uproutbtn_Click(object sender, EventArgs e)
        {
            if (IDtxt.Text.Length != 0 && DisTXT.Text.Length != 0 && RouteTXT.Text.Length != 0&& UPRTXT.Text.Length!=0)
            {
                Update_Route(IDtxt.Text, RouteTXT.Text, UPRTXT.Text, Convert.ToDouble(DisTXT.Text));
                IDtxt.Text = "";
                RouteTXT.Text = "";
                DisTXT.Text = "";
                UPRTXT.Text = "";

                dt2.Rows.Clear();
                for (int i = 0; i < Length_R(); i++)
                {
                    StringBuilder stb = new StringBuilder(1000);
                    Read_Route(stb, (i + 1));
                    String[] a = stb.ToString().Split(';');

                    dt2.Rows.Add(a[0], a[1], a[2]);
                }

            }
            else
            {
                MessageBox.Show("Check the fields");
            }
        }
    }
}
