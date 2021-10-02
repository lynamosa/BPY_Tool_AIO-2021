using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using corel = Corel.Interop.VGCore;

namespace BPYmergeTool
{

    public partial class DockerUI : UserControl
    {
        private corel.Application corelApp;
        public DockerUI(corel.Application app)
        {
            this.corelApp = app;
            InitializeComponent();
        }
        private void st_btn_pg_rotate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello world!");
        }

        private void st_btn_pg_rotate_Click_1(object sender, RoutedEventArgs e)
        {
            string x = st_pg_width.Text;
            st_pg_width.Text = st_pg_height.Text;
            st_pg_height.Text = x;
        }
        public void hellow()
        {
            MessageBox.Show("Hello world");
        }
        private void st_btn_getItemSize_Click(object sender, RoutedEventArgs e)
        {
            corelApp.ActiveDocument.Unit = corel.cdrUnit.cdrMillimeter;
            st_item_width.Text = Math.Round(corelApp.ActiveSelection.SizeWidth, 3).ToString();
            st_item_height.Text = Math.Round(corelApp.ActiveSelection.SizeHeight, 3).ToString();
        }
        private void st_btn_size_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello world!");
        }
        private void st_btn_SortCount_click(object sender, RoutedEventArgs e)
        {
            float pw = float.Parse(st_pg_width.Text);
            float ph = float.Parse(st_pg_height.Text);
            float iw = float.Parse(st_item_width.Text);
            float ih = float.Parse(st_item_height.Text);
            int nX = 1;
            int nY = 1;
            int rX = 0;
            int rY = 0;
            int valueMax = 0;
            int indexMax = 0;
            if (st_chk_ro.IsChecked == true)
            {
                for (int i = 0; i < Math.Floor(pw / iw); i++)
                {
                    nX = i;
                    nY = (int)Math.Floor(ph / ih);
                    rX = (int)Math.Floor((pw - nX * iw) / ih);
                    rY = (int)Math.Floor(ph / iw);
                    if (nX * nY + rX * rY > valueMax)
                    {
                        indexMax = i;
                        valueMax = nX * nY + rX * rY;
                    }
                }
                st_x.Text = indexMax.ToString();
                st_y.Text = Math.Floor(ph / ih).ToString();
                st_rx.Text = Math.Floor((pw - indexMax * iw) / ih).ToString();
                st_ry.Text = Math.Floor(ph / iw).ToString();
            }
            else
            {
                st_x.Text = Math.Floor(pw / iw).ToString();
                st_y.Text = Math.Floor(ph / ih).ToString();
                st_rx.Text = "0";
                st_ry.Text = "0";
            }
        }

        public void obj2page()
        {
            corelApp.Unit = corel.cdrUnit.cdrMillimeter;
            corel.Shapes sRange;
            corelApp.ActiveDocument.BeginCommandGroup("Sort Object to page");
            corelApp.Optimization = true;
            sRange = corelApp.ActiveSelection.Shapes;
            int i, j;
            int count = 0;
            float tmp;
            float rX = 0;
            float pW = float.Parse(st_pg_width.Text);
            float pH = float.Parse(st_pg_height.Text);
            float sW = float.Parse(st_item_width.Text);
            float sH = float.Parse(st_item_height.Text);
            float sX = float.Parse(st_x.Text);
            float sY = float.Parse(st_y.Text);
            float snX = float.Parse(st_rx.Text);
            float snY = float.Parse(st_ry.Text);
            float sSpace = float.Parse(st_space.Text);
            int k_direct_type = (st_chk_dr1.IsChecked == true) ? 1 : 0;
            // Code here
            if (k_direct_type != 0)
            {
                tmp = sX;
                sX = sY;
                sY = tmp;

                tmp = snX;
                snX = snY;
                snY = tmp;
            }
            do
            {
                for (i = 0; i < sX; i++)
                {
                    for (j = 0; j < sY; j++)
                    {
                        count++;
                        if (count > sRange.Count)
                        {
                            i = (int)sX + 1;
                            break;
                        }
                        if (k_direct_type == 0)
                        {
                            sRange[count].LeftX = rX + i * sW;
                            sRange[count].TopY = -20 - j * sH;
                        }
                        else
                        {
                            sRange[count].LeftX = rX + j * sW;
                            sRange[count].TopY = -20 - i * sH;
                        }
                    }
                }
                if (k_direct_type != 0)
                {
                    rX += sY * sW;
                }
                else
                {
                    rX += sX * sW;
                }
                for (i = 0; i < snX; i++)
                {
                    for (j = 0; j < snY; j++)
                    {
                        count++;
                        if (count > sRange.Count)
                        {
                            i = (int)snX + 1; // break loop i
                            break;
                        }
                        sRange[count].Rotate(90);
                        if (k_direct_type == 0)
                        {
                            sRange[count].LeftX = rX + i * sH;
                            sRange[count].TopY = -20 - j * sW;
                        }
                        else
                        {
                            sRange[count].LeftX = rX + j * sH;
                            sRange[count].TopY = -20 - i * sW;
                        }
                    }
                }
                rX += sSpace;
            } while (count < sRange.Count);

            //End Code
            corelApp.Optimization = false;
            corelApp.ActiveDocument.EndCommandGroup();
            corelApp.Refresh();
        }
        public void obj_sort(int sType) { 
            corelApp.Unit = corel.cdrUnit.cdrMillimeter;
            corel.Shapes sRange;
            if (sType == 1)
            {
                corelApp.ActiveDocument.BeginCommandGroup("Sort All Object");
                sRange = corelApp.ActivePage.Shapes;
            }
            else
            {
                corelApp.ActiveDocument.BeginCommandGroup("Sort Selected Object");
                sRange = corelApp.ActiveSelection.Shapes;
            }
            int i, j;
            int count = 0;
            float tmp;
            float rX = 0;
            float pW = float.Parse(st_pg_width.Text);
            float pH = float.Parse(st_pg_height.Text);
            float sW = float.Parse(st_item_width.Text);
            float sH = float.Parse(st_item_height.Text);
            float sX = float.Parse(st_x.Text);
            float sY = float.Parse(st_y.Text);
            float snX = float.Parse(st_rx.Text);
            float snY = float.Parse(st_ry.Text);
            float sSpace = float.Parse(st_space.Text);
            corelApp.Optimization = true;
            int k_direct_type = (st_chk_dr1.IsChecked == true) ? 1 : 0;
            // Code here
            if (k_direct_type != 0)
            {
                tmp = sX;
                sX = sY;
                sY = tmp;

                tmp = snX;
                snX = snY;
                snY = tmp;
            }
            do
            {
                for (i = 0; i < sX; i++)
                {
                    for (j = 0; j < sY; j++)
                    {
                        count++;
                        if (count > sRange.Count)
                        {
                            i = (int)sX + 1;
                            break;
                        }
                        if (k_direct_type == 0)
                        {
                            sRange[count].LeftX = rX + i * sW;
                            sRange[count].TopY = -20 - j * sH;
                        }
                        else
                        {
                            sRange[count].LeftX = rX + j * sW;
                            sRange[count].TopY = -20 - i * sH;
                        }
                    }
                }
                if (k_direct_type != 0)
                {
                    rX += sY * sW;
                }
                else
                {
                    rX += sX * sW;
                }
                for (i = 0; i < snX; i++)
                {
                    for (j = 0; j < snY; j++)
                    {
                        count++;
                        if (count > sRange.Count)
                        {
                            i = (int)snX + 1; // break loop i
                            break;
                        }
                        sRange[count].Rotate(90);
                        if (k_direct_type == 0)
                        {
                            sRange[count].LeftX = rX + i * sH;
                            sRange[count].TopY = -20 - j * sW;
                        }
                        else
                        {
                            sRange[count].LeftX = rX + j * sH;
                            sRange[count].TopY = -20 - i * sW;
                        }
                    }
                }
                if (k_direct_type != 0)
                {
                    rX += snY * sH + sSpace;
                }
                else
                {
                    rX += snX * sH + sSpace;
                }
            } while (count < sRange.Count);

            //End Code
            corelApp.Optimization = false;
            corelApp.ActiveDocument.EndCommandGroup();
            corelApp.Refresh();
            MessageBox.Show("Finish");
        }
        public void page_sort(int sType)
        {
            string[] nameType = new string[] { "Sort all page", "Sort odd page", "Sort even page" };
            MessageBox.Show(nameType[1]);
            corelApp.ActiveDocument.BeginCommandGroup(nameType[sType]);
            corelApp.Optimization = true;

            corelApp.Optimization = false;
            corelApp.ActiveDocument.EndCommandGroup();
        }
        private void st_btn_sort_Click(object sender, RoutedEventArgs e)
        {
            int sType = st_type.SelectedIndex;
            corelApp.Unit = corel.cdrUnit.cdrMillimeter;

            switch (sType)
            {
                //------------------------- All Object ------------------------//
                case 0: //"Tất cả các đối tượng":
                    obj_sort(1);
                    break;
                //--------------------- Selected Object ----------------------//
                case 1: // "Chỉ đối tượng được chọn":
                    obj_sort(0);
                    break;
                //------------------------- All Page -------------------------//
                case 2: // "Tất cả các trang":
                    page_sort(0);
                    break;
                //------------------------ Odd Object ------------------------//
                case 3: // "Chỉ trang chẵn":
                    page_sort(1);
                    break;
                //------------------------ Even Object -----------------------//
                case 4: // "Chỉ trang lẻ":
                    page_sort(2);
                    break;
            }
        }
        //----------Tab MOVE-----------//
        public corel.Document sRange { get; set; }
        private void sz_btn_scale_auto(object sender, RoutedEventArgs e)
        {
            float sType = float.Parse(((Button)e.OriginalSource).Tag.ToString());
            float x = float.Parse(sz_width.Text);
            float y = float.Parse(sz_height.Text);
            Boolean r = sz_chk_rotate.IsChecked.Value;
            corelApp.ActiveDocument.Unit = corel.cdrUnit.cdrMillimeter;
            if (corelApp.ActiveSelection.Shapes.Count < 1)
                return;
            corelApp.ActiveDocument.BeginCommandGroup("Scale shape");
            corelApp.Optimization = true;
            corelApp.ActiveDocument.ReferencePoint = corel.cdrReferencePoint.cdrCenter;
            switch (sType)
            {
                case 1:
                    for (int i = 1; i <= corelApp.ActiveSelection.Shapes.Count; i++)
                    {
                        corelApp.ActiveSelection.Shapes[i].Stretch((double)(x / corelApp.ActiveSelection.Shapes[i].SizeWidth));
                    }

                    break;
                case 2:
                    for (int i = 1; i <= corelApp.ActiveSelection.Shapes.Count; i++)
                    {
                        corelApp.ActiveSelection.Shapes[i].Stretch((double)(y / corelApp.ActiveSelection.Shapes[i].SizeHeight));
                    }

                    break;
                case 3:
                    for (int i = 1; i <= corelApp.ActiveSelection.Shapes.Count; i++)
                    {
                        corelApp.ActiveSelection.Shapes[i].SizeWidth = x;
                    }

                    break;
                case 4:
                    for (int i = 1; i <= corelApp.ActiveSelection.Shapes.Count; i++)
                        corelApp.ActiveSelection.Shapes[i].SizeHeight = y;
                    break;
                case 5:
                    for (int i = 1; i <= corelApp.ActiveSelection.Shapes.Count; i++)
                    {
                        if (r == true)
                        {
                            if (corelApp.ActiveSelection.Shapes[i].SizeWidth / corelApp.ActiveSelection.Shapes[i].SizeHeight > 1 && x / y < 1)
                                corelApp.ActiveSelection.Shapes[i].Rotate(90);
                            else if (corelApp.ActiveSelection.Shapes[i].SizeWidth / corelApp.ActiveSelection.Shapes[i].SizeHeight < 1 && x / y > 1)
                                corelApp.ActiveSelection.Shapes[i].Rotate(90);
                        }
                        if (corelApp.ActiveSelection.Shapes[i].SizeHeight / y < corelApp.ActiveSelection.Shapes[i].SizeWidth / x)
                            corelApp.ActiveSelection.Shapes[i].Stretch((double)(x / corelApp.ActiveSelection.Shapes[i].SizeWidth));
                        else
                            corelApp.ActiveSelection.Shapes[i].Stretch((double)(y / corelApp.ActiveSelection.Shapes[i].SizeHeight));
                    }
                    break;
                case 6:
                    for (int i = 1; i <= corelApp.ActiveSelection.Shapes.Count; i++)
                    {
                        if (r == true)
                        {
                            if (corelApp.ActiveSelection.Shapes[i].SizeWidth / corelApp.ActiveSelection.Shapes[i].SizeHeight > 1 && x / y < 1)
                                corelApp.ActiveSelection.Shapes[i].Rotate(90);
                            else if (corelApp.ActiveSelection.Shapes[i].SizeWidth / corelApp.ActiveSelection.Shapes[i].SizeHeight < 1 && x / y > 1)
                                corelApp.ActiveSelection.Shapes[i].Rotate(90);
                        }
                        if (corelApp.ActiveSelection.Shapes[i].SizeHeight / y > corelApp.ActiveSelection.Shapes[i].SizeWidth / x)
                            corelApp.ActiveSelection.Shapes[i].Stretch((double)(x / corelApp.ActiveSelection.Shapes[i].SizeWidth));
                        else
                            corelApp.ActiveSelection.Shapes[i].Stretch((double)(y / corelApp.ActiveSelection.Shapes[i].SizeHeight));
                    }
                    break;
                case 7:
                    for (int i = 1; i <= corelApp.ActiveSelection.Shapes.Count; i++)
                    {
                        corelApp.ActiveSelection.Shapes[i].SizeWidth = x;
                        corelApp.ActiveSelection.Shapes[i].SizeHeight = y;
                    }
                    break;
                default:
                    break;
            }
            corelApp.Optimization = false;
            corelApp.ActiveDocument.EndCommandGroup();
            corelApp.Refresh();
        }

        private void sz_btn_getIndex_Click(object sender, RoutedEventArgs e)
        {
            if (corelApp.ActiveSelection.Shapes.Count != 1)
                MessageBox.Show("Vui lòng chọn 1 đối tượng", "Lỗi");
            else
                sz_obj_Index.Text = corelApp.ActivePage.Shapes.All().IndexOf(corelApp.ActiveSelection.Shapes[1]).ToString();
        }

        private void sz_btn_size_Click(object sender, RoutedEventArgs e)
        {
            corelApp.ActiveDocument.BeginCommandGroup("Auto fix Size");
            corelApp.Optimization = true;
            corel.ShapeRange sr;
            int stt = int.Parse(sz_obj_Index.Text);
            foreach (corel.Page p in corelApp.ActiveDocument.Pages)
            {
                p.Activate();
                sr = corelApp.ActivePage.FindShapes("", corel.cdrShapeType.cdrTextShape);
                sr.ConvertToCurves();
                foreach (corel.Shape s in corelApp.ActivePage.Shapes.FindShapes(Query: "!@com.powerclip.IsNull"))
                {
                    sr = s.PowerClip.Shapes.FindShapes("", corel.cdrShapeType.cdrTextShape);
                    sr.ConvertToCurves();
                }
            }
            
        }

        private void sz_att_left_Click(object sender, RoutedEventArgs e)
        {
            double al_hor_space, al_ver_space, al_left, al_top;
            ADODB.Recordset rs = new ADODB.Recordset();
            rs.LockType = ADODB.LockTypeEnum.adLockOptimistic;
            rs.CursorType = ADODB.CursorTypeEnum.adOpenDynamic;
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient;
            corel.ShapeRange sel;
            corelApp.ActiveDocument.Unit = corel.cdrUnit.cdrMillimeter;
            sel = corelApp.ActiveSelectionRange;
            if (sel.Shapes.Count <= 1)
            {
                MessageBox.Show("Vui lòng chọn 2 đối tượng trở lên", "Không thể sắp xếp!");
            }
            else
            {
                corelApp.ActiveDocument.BeginCommandGroup("Align shape");
                corelApp.Optimization = true;
                for (int c = 0; c <= 8; c++)
                {
                    rs.Fields.Append("Fld" + c, ADODB.DataTypeEnum.adDouble);
                }
                rs.Open();
                for (int i = 1; i <= sel.Shapes.Count; i++)
                {
                    rs.AddNew();
                    rs.Fields[0].Value = (double)i;
                    rs.Fields[1].Value = sel.Shapes[i].LeftX;
                    rs.Fields[2].Value = sel.Shapes[i].LeftX + sel.Shapes[i].SizeWidth;
                    rs.Fields[3].Value = sel.Shapes[i].TopY;
                    rs.Fields[4].Value = sel.Shapes[i].TopY + sel.Shapes[i].SizeHeight;
                    rs.Fields[5].Value = sel.Shapes[i].LeftX + sel.Shapes[i].SizeWidth / 2;
                    rs.Fields[6].Value = sel.Shapes[i].TopY + sel.Shapes[i].SizeHeight / 2;
                    rs.Fields[7].Value = sel.Shapes[i].SizeWidth;
                    rs.Fields[8].Value = sel.Shapes[i].SizeHeight;
                    rs.Update();
                }
                string direct = sz_cb_sortdesc.SelectedIndex == 1 ? " ASC" : " DESC";
                rs.Sort = "Fld" + sz_cb_sortType.SelectedIndex + direct;
                rs.MoveFirst();

                al_left = sel.LeftX;
                al_top = sel.TopY;
                al_hor_space = double.Parse(sz_hor.Text);
                al_ver_space = double.Parse(sz_ver.Text);
                switch (sender.ToString().Substring(32))
                {
                    case "LEFT":
                        for (int z = 0; z < sel.Shapes.Count; z++)
                        {
                            sel.Shapes[rs.Fields[0].Value].LeftX = al_left;
                            al_left += al_hor_space;
                            rs.MoveNext();
                        }
                        break;
                    case "CENTER":
                        for (int z = 0; z < sel.Shapes.Count; z++)
                        {
                            sel.Shapes[rs.Fields[0].Value].LeftX = al_left + al_hor_space - sel.Shapes[rs.Fields[0].Value].SizeWidth / 2;
                            al_left = al_left + al_hor_space;
                            rs.MoveNext();
                        }
                        break;
                    case "RIGHT":
                        for (int z = 0; z < sel.Shapes.Count; z++)
                        {
                            sel.Shapes[rs.Fields[0].Value].LeftX = al_left + al_hor_space - sel.Shapes[rs.Fields[0].Value].SizeWidth;
                            al_left = al_left + al_hor_space;
                            rs.MoveNext();
                        }
                        break;
                    case "HOR":
                        for (int z = 0; z < sel.Shapes.Count; z++)
                        {
                            sel.Shapes[rs.Fields[0].Value].LeftX = al_left;
                            al_left = al_left + sel.Shapes[rs.Fields[0].Value].SizeWidth + al_hor_space;
                            rs.MoveNext();
                        }
                        break;
                    case "TOP":
                        for (int z = 0; z < sel.Shapes.Count; z++)
                        {
                            sel.Shapes[rs.Fields[0].Value].TopY = al_top;
                            al_top = al_top - al_ver_space;
                            rs.MoveNext();
                        }
                        break;
                    case "MID":
                        for (int z = 0; z < sel.Shapes.Count; z++)
                        {
                            sel.Shapes[rs.Fields[0].Value].TopY = al_top - al_ver_space + sel.Shapes[rs.Fields[0].Value].SizeHeight / 2;
                            al_top = al_top - al_ver_space;
                            rs.MoveNext();
                        }
                        break;
                    case "BOTTOM":
                        for (int z = 0; z < sel.Shapes.Count; z++)
                        {
                            sel.Shapes[rs.Fields[0].Value].TopY = al_top - al_ver_space + sel.Shapes[rs.Fields[0].Value].SizeHeight;
                            al_top = al_top - al_ver_space;
                            rs.MoveNext();
                        }
                        break;
                    case "VER":
                        for (int z = 0; z < sel.Shapes.Count; z++)
                        {
                            sel.Shapes[rs.Fields[0].Value].TopY = al_top;
                            al_top = al_top - sel.Shapes[rs.Fields[0].Value].SizeHeight - al_ver_space;
                            rs.MoveNext();
                        }
                        break;
                }
                rs.Close();
                corelApp.Optimization = false;
                corelApp.ActiveDocument.EndCommandGroup();
                corelApp.Refresh();
            }
        }

        private void sz_att_center_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sz_cb_sortType.SelectedIndex.ToString());
        }

        private void btn_crackFont_Click(object sender, RoutedEventArgs e)
        {
            float sType = float.Parse(((Button)e.OriginalSource).Tag.ToString());
            corelApp.ActiveDocument.BeginCommandGroup("Crack Font");
            corelApp.Optimization = true;
            corel.ShapeRange sr;

            if (sType==1)
            {
                foreach (corel.Page p in corelApp.ActiveDocument.Pages)
                {
                    p.Activate();
                    sr = corelApp.ActivePage.FindShapes("", corel.cdrShapeType.cdrTextShape);
                    sr.ConvertToCurves();
                    foreach (corel.Shape s in corelApp.ActivePage.Shapes.FindShapes(Query:"!@com.powerclip.IsNull"))
                    {
                        sr = s.PowerClip.Shapes.FindShapes("", corel.cdrShapeType.cdrTextShape);
                        sr.ConvertToCurves();
                    }
                }
            }
            else if (sType==2)
            {
                sr = corelApp.ActivePage.FindShapes("", corel.cdrShapeType.cdrTextShape);
                sr.ConvertToCurves();
                foreach (corel.Shape s in corelApp.ActivePage.Shapes.FindShapes(Query: "!@com.powerclip.IsNull"))
                {
                    sr = s.PowerClip.Shapes.FindShapes("", corel.cdrShapeType.cdrTextShape);
                    sr.ConvertToCurves();
                }
            }
            else if(corelApp.ActiveSelection.Shapes.Count>0)
            {
                
                sr = corelApp.ActiveSelection.Shapes.FindShapes("", corel.cdrShapeType.cdrTextShape);
                sr.ConvertToCurves();
                foreach (corel.Shape s in corelApp.ActiveSelection.Shapes.FindShapes(Query: "!@com.powerclip.IsNull"))
                {
                    sr = s.PowerClip.Shapes.FindShapes("", corel.cdrShapeType.cdrTextShape);
                    sr.ConvertToCurves();
                }
            }
            corelApp.Optimization = false;
            corelApp.ActiveDocument.EndCommandGroup();
            corelApp.Refresh();
        }

        private void btn_barcode2vector_Click(object sender, RoutedEventArgs e)
        {
            if (corelApp.ActiveSelection.Shapes.Count!=1)
            {
                MessageBox.Show("Vui lòng chọn 1 đối tượng");
            }
            else
            {
                if (corelApp.ActiveSelection.Shapes[1].Type != corel.cdrShapeType.cdrOLEObjectShape)
                {
                    MessageBox.Show("Vui lòng chọn đối tượng barcode \n" + corelApp.ActiveSelection.Shapes[1].Type);
                }
                else
                {
                    corelApp.ActiveDocument.BeginCommandGroup("Crack Font");
                    corelApp.Optimization = true;

                    double x = corelApp.ActiveSelection.LeftX;
                    double y = corelApp.ActiveSelection.TopY;
                    corelApp.ActiveSelection.Cut();
                    corelApp.ActiveLayer.PasteSpecial("Metafile");
                    corelApp.ActiveSelection.LeftX = x;
                    corelApp.ActiveSelection.TopY = y;

                    corelApp.Optimization = false;
                    corelApp.ActiveDocument.EndCommandGroup();
                    corelApp.Refresh();
                }
            }
        }

        private void btn_resampImg_Click(object sender, RoutedEventArgs e)
        {
            corelApp.ActiveDocument.BeginCommandGroup("Resample image");
            corelApp.Optimization = true;
            float maxDPI = float.Parse(txt_maxdpi.Text);
            if (corelApp.ActiveSelection.Shapes.Count > 0)
            {
                foreach (corel.Shape s in corelApp.ActiveSelection.Shapes.FindShapes("", corel.cdrShapeType.cdrBitmapShape))
                {
                    if (s.Bitmap.ResolutionX > maxDPI || s.Bitmap.ResolutionY > maxDPI)
                    {
                        s.Bitmap.Crop();
                        s.Bitmap.Resample(ResolutionX: maxDPI, ResolutionY: maxDPI);
                    }
                }
                foreach (corel.Shape sr in corelApp.ActiveSelection.Shapes.FindShapes(Query: "!@com.powerclip.IsNull"))
                {
                    foreach (corel.Shape s in sr.PowerClip.Shapes.FindShapes("", corel.cdrShapeType.cdrBitmapShape))
                    {
                        if (s.Bitmap.ResolutionX > maxDPI || s.Bitmap.ResolutionY > maxDPI)
                        {
                            s.Bitmap.Crop();
                            s.Bitmap.Resample(ResolutionX: maxDPI, ResolutionY: maxDPI);
                        }
                    }
                }
            }
            else
            {
                foreach (corel.Page page in corelApp.ActiveDocument.Pages)
                {
                    foreach (corel.Shape s in page.Shapes.FindShapes("", corel.cdrShapeType.cdrBitmapShape))
                    {
                        if (s.Bitmap.ResolutionX > maxDPI || s.Bitmap.ResolutionY > maxDPI)
                        {
                            s.Bitmap.Crop();
                            s.Bitmap.Resample(ResolutionX: maxDPI, ResolutionY: maxDPI);
                        }
                    }
                    foreach (corel.Shape sr in page.Shapes.FindShapes(Query: "!@com.powerclip.IsNull"))
                    {
                        foreach (corel.Shape s in sr.PowerClip.Shapes.FindShapes("", corel.cdrShapeType.cdrBitmapShape))
                        {
                            if (s.Bitmap.ResolutionX > maxDPI || s.Bitmap.ResolutionY > maxDPI)
                            {
                                s.Bitmap.Crop();
                                s.Bitmap.Resample(ResolutionX: maxDPI, ResolutionY: maxDPI);
                            }
                        }
                    }
                }
            }
            corelApp.Optimization = false;
            corelApp.ActiveDocument.EndCommandGroup();
            corelApp.Refresh();
        }

        private void sz_btn_align_Click(object sender, RoutedEventArgs e)
        {
            corelApp.ActiveDocument.BeginCommandGroup("Auto Align");
            corelApp.Optimization = true;
            corel.ShapeRange sr;
            int rootShape = int.Parse(sz_al_ori_index.Text);
            int moveShape = int.Parse(sz_al_des_index.Text);
            int alignType = 0;
            int[] vAlign = { 4, 8, 12 };
            int[] hAlign = { 1, 2, 3 };
            if (sz_chk_hoz.IsChecked == true)
            {
                alignType += (sz_cb_hoz.SelectedIndex + 1);
            }
            if (sz_chk_ver.IsChecked == true)
            {
                alignType += vAlign[sz_cb_ver.SelectedIndex];
            }
            if (alignType==0)
            {
                MessageBox.Show("Vui lòng chọn kiểu", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (corel.Page p in corelApp.ActiveDocument.Pages)
            {
                p.Activate();
                if (sz_chk_hoz.IsChecked == true)
                {
                    corelApp.ActiveLayer.Shapes[moveShape].AlignToShape(corel.cdrAlignType.cdrAlignLeft, corelApp.ActiveLayer.Shapes[rootShape]);
                }
                if (sz_chk_ver.IsChecked == true)
                {
                    alignType += vAlign[sz_cb_ver.SelectedIndex];
                }
                if (sz_chk_hoz.IsChecked==true)
                {
                    corelApp.ActiveLayer.Shapes[moveShape].AlignToShape(corel.cdrAlignType.cdrAlignLeft, corelApp.ActiveLayer.Shapes[rootShape]);
                }

                sr = corelApp.ActivePage.FindShapes("", corel.cdrShapeType.cdrTextShape);
                sr.ConvertToCurves();
                foreach (corel.Shape s in corelApp.ActivePage.Shapes.FindShapes(Query: "!@com.powerclip.IsNull"))
                {
                    sr = s.PowerClip.Shapes.FindShapes("", corel.cdrShapeType.cdrTextShape);
                    sr.ConvertToCurves();
                }
            }
        }
    }
}
