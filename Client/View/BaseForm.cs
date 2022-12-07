﻿using Client.Callout;
using Client.Dtos.Product;
using Client.Dtos.UserDto;
using Client.Enumerates;
using Client.Utills;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.View
{
    public partial class BaseForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static BaseForm _instanceBaseForm;
        public StateEnum _state = StateEnum.NONE;

        public int _pageIndex = 1;
        public int _pageSize = 15;
        public object _dataRow = null;

        public BaseForm()
        {
            InitializeComponent();
            _instanceBaseForm = this;

            BuildFromByRole(Login._instance._role);
            _ = LoadDataGrid();
        }

        private void BuildFromByRole(string role)
        {
            switch (role)
            {
                case "admin":
                    break;
                default:
                    ribbonPageUsers.Visible = false;
                    break;
            }
        }

        private void Clear()
        {
            _pageIndex = 1;
            _pageSize = 15;
            dataGrid.DataSource = null;
            gridView1.Columns.Clear();
            _dataRow = null;
        }

        private async Task LoadDataGrid()
        {
            SetVisiblePaging(true);

            if (ribbon.SelectedPage == ribbonPageManagerProducts)
            {
                var result = await new ProductsService().GetProducts(new GetManageProductPagingRequest()
                {
                    PageIndex = _pageIndex,
                    PageSize = _pageSize,
                    Keyword = String.Empty,
                });

                dataGrid.DataSource = result.Items;
                _dataRow = result.Items.Count > 0 ? result.Items[0] : null;
                SetPaging(result.TotalRecords, result.PageIndex, result.PageSize);
            }

            if (ribbon.SelectedPage == ribbonPageCarts)
            {

            }

            if (ribbon.SelectedPage == ribbonPageCategories)
            {
                var result = await new CategoriesService().GetCategories("vi");
                SetVisiblePaging(false);
                dataGrid.DataSource = result;
                gridView1.Columns[0].Visible = false;

            }

            if (ribbon.SelectedPage == ribbonPageUsers)
            {
                var result = await new UsersService().GetPagingUsers(new GetUserPagingRequest()
                {
                    PageIndex = _pageIndex,
                    PageSize = _pageSize,
                    Keyword = String.Empty,
                });

                dataGrid.DataSource = result.ResultObj.Items;
                _dataRow = result.ResultObj.Items.Count > 0 ? result.ResultObj.Items[0] : null;
                SetPaging(result.ResultObj.TotalRecords, result.ResultObj.PageIndex, result.ResultObj.PageSize);
            }
        }

        private void SetVisiblePaging(bool isVisible)
        {
            if (!isVisible)
            {
                labTotal.Visibility = LayoutVisibility.Never;
                labCombPageSize.Visibility = LayoutVisibility.Never;
                layoutControl_btnBackPage.Visibility = LayoutVisibility.Never;
                layoutControlI_btnNextPage.Visibility = LayoutVisibility.Never;
            }
            else
            {
                labTotal.Visibility = LayoutVisibility.Always;
                labCombPageSize.Visibility = LayoutVisibility.Always;
                layoutControl_btnBackPage.Visibility = LayoutVisibility.Always;
                layoutControlI_btnNextPage.Visibility = LayoutVisibility.Always;
            }
        }

        private void SetPaging(int totalRecords, int pageIndex, int pageSize)
        {
            gridView1.Columns[0].Visible = false;
            combPageSize.Text = pageSize.ToString();
            labTotal.Text = "Tổng số bản ghi: " + totalRecords.ToString();
            labCombPageSize.Text = "Số bản ghi trang " + pageIndex;
        }

        private async void ribbon_SelectedPageChanged(object sender, EventArgs e)
        {
            Clear();
            await LoadDataGrid();
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e) => _dataRow = (sender as GridView).FocusedRowObject;

        private void btnAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            _state = StateEnum.INSERT;
            InitFromInsertOrUpdate();
        }

        private void btnUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            _state = StateEnum.UPDATE;
            InitFromInsertOrUpdate();
        }

        private void InitFromInsertOrUpdate()
        {
            if (_state == StateEnum.NONE || _dataRow == null)
            {
                MessageBoxUtil.ShowMessageBox("Lỗi", "Hệ thống tạm thời gián đoạn, vui lòng thử lại sau.", MessageBoxType.Error);
                return;
            }

            if (ribbon.SelectedPage == ribbonPageManagerProducts)
            {
            }

            if (ribbon.SelectedPage == ribbonPageUsers)
            {
                UserFrom userFrom = new UserFrom();
                userFrom.ShowDialog();
            }
        }

        private async void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_dataRow == null)
            {
                MessageBoxUtil.ShowMessageBox("Thông báo", "Vui lòng chọn 1 bản ghi để xoá..", MessageBoxType.Information);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Xác nhận", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                if (ribbon.SelectedPage == ribbonPageManagerProducts)
                {
                    var result = await new ProductsService().DeleteProduct((_dataRow as ProductVm).Id);
                }

                if (ribbon.SelectedPage == ribbonPageUsers)
                {
                    var result = await new UsersService().DeleteUser((_dataRow as UserVm).Id);
                    if (!result.IsSuccessed)
                    {
                        MessageBoxUtil.ShowMessageBox("Error", result.Message, MessageBoxType.Error);
                        return;
                    }
                }

                await LoadDataGrid();
            }
        }

        private async void combPageSize_SelectedValueChanged(object sender, EventArgs e)
        {
            _pageSize = int.Parse(combPageSize.SelectedItem.ToString());
            await LoadDataGrid();
        }

        private async void btnNextPage_Click(object sender, EventArgs e)
        {
            _pageIndex ++;
            await LoadDataGrid();
        }

        private async void btnBackPage_Click(object sender, EventArgs e)
        {
            if (_pageIndex > 0)
            {
                _pageIndex--;
                await LoadDataGrid();
            }    
        }
    }
}