using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM.Winforms
{
    public partial class Form1 : Form
    {
        private const string ApiBaseUrl =
            "http://localhost:5080/api/datacollection";

        private readonly HttpClient httpClient = new HttpClient();

        private ComboBox cmbTenant = null!;

        private TextBox txtName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhone = null!;
        private TextBox txtAddress = null!;
        private ComboBox cmbCustomerType = null!;
        private TextBox txtFavoriteFood = null!;
        private ComboBox cmbVisitFrequency = null!;
        private TextBox txtFeedback = null!;

        private Button btnRegister = null!;
        private Button btnCancelEdit = null!;
        private Button btnRefresh = null!;
        private Button btnMasterView = null!;

        private DataGridView dgvCustomers = null!;

        private Label lblStatus = null!;
        private Label lblRecordCount = null!;

        private int selectedCustomerId = 0;
        private bool isEditing = false;

        public Form1()
        {
            BuildInterface();
            Load += async (sender, e) => await LoadCustomers();
        }

        // =========================================================
        // BUILD INTERFACE
        // =========================================================

        private void BuildInterface()
        {
            Text = "CRM Restaurant System - Customer Data Collection";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1000, 650);

            BackColor = Color.White;

            // =====================================================
            // MAIN PANEL
            // =====================================================

            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            Controls.Add(mainPanel);

            // =====================================================
            // TITLE
            // =====================================================

            Label lblTitle = new Label
            {
                Text = "CRM RESTAURANT SYSTEM",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 53, 88),
                AutoSize = true,
                Location = new Point(20, 12)
            };

            mainPanel.Controls.Add(lblTitle);

            Label lblSubtitle = new Label
            {
                Text = "Customer Data Collection",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(22, 58)
            };

            mainPanel.Controls.Add(lblSubtitle);

            // =====================================================
            // REGISTRATION PANEL
            // =====================================================

            Panel registrationPanel = new Panel
            {
                Location = new Point(20, 90),
                Height = 250,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(245, 248, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            mainPanel.Controls.Add(registrationPanel);

            Label lblRegistration = new Label
            {
                Text = "CUSTOMER REGISTRATION",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 53, 88),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            registrationPanel.Controls.Add(lblRegistration);

            // =====================================================
            // TENANT
            // =====================================================

            AddLabel(registrationPanel, "Restaurant / Tenant", 20, 55);

            cmbTenant = new ComboBox
            {
                Location = new Point(20, 80),
                Width = 140,
                Height = 30,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbTenant.Items.Add("A");
            cmbTenant.Items.Add("B");
            cmbTenant.Items.Add("C");
            cmbTenant.SelectedIndex = 0;

            registrationPanel.Controls.Add(cmbTenant);

            // =====================================================
            // ROW 1
            // =====================================================

            AddLabel(registrationPanel, "Full Name *", 180, 55);

            txtName = new TextBox
            {
                Location = new Point(180, 80),
                Width = 220,
                Height = 30
            };

            registrationPanel.Controls.Add(txtName);

            AddLabel(registrationPanel, "Email (Optional)", 420, 55);

            txtEmail = new TextBox
            {
                Location = new Point(420, 80),
                Width = 220,
                Height = 30
            };

            registrationPanel.Controls.Add(txtEmail);

            AddLabel(registrationPanel, "Phone Number *", 660, 55);

            txtPhone = new TextBox
            {
                Location = new Point(660, 80),
                Width = 180,
                Height = 30
            };

            registrationPanel.Controls.Add(txtPhone);

            // =====================================================
            // ROW 2
            // =====================================================

            AddLabel(registrationPanel, "Address *", 20, 125);

            txtAddress = new TextBox
            {
                Location = new Point(20, 150),
                Width = 220,
                Height = 30
            };

            registrationPanel.Controls.Add(txtAddress);

            AddLabel(registrationPanel, "Customer Type *", 260, 125);

            cmbCustomerType = new ComboBox
            {
                Location = new Point(260, 150),
                Width = 160,
                Height = 30,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbCustomerType.Items.Add("Regular");
            cmbCustomerType.Items.Add("New");
            cmbCustomerType.Items.Add("VIP");

            registrationPanel.Controls.Add(cmbCustomerType);

            AddLabel(registrationPanel, "Favorite Food", 440, 125);

            txtFavoriteFood = new TextBox
            {
                Location = new Point(440, 150),
                Width = 220,
                Height = 30
            };

            registrationPanel.Controls.Add(txtFavoriteFood);

            AddLabel(registrationPanel, "Visit Frequency *", 680, 125);

            cmbVisitFrequency = new ComboBox
            {
                Location = new Point(680, 150),
                Width = 160,
                Height = 30,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbVisitFrequency.Items.Add("Daily");
            cmbVisitFrequency.Items.Add("Weekly");
            cmbVisitFrequency.Items.Add("Monthly");
            cmbVisitFrequency.Items.Add("Occasionally");
            cmbVisitFrequency.Items.Add("First Visit");

            registrationPanel.Controls.Add(cmbVisitFrequency);

            // =====================================================
            // ROW 3
            // =====================================================

            AddLabel(registrationPanel, "Feedback", 20, 195);

            txtFeedback = new TextBox
            {
                Location = new Point(20, 220),
                Width = 440,
                Height = 30
            };

            registrationPanel.Controls.Add(txtFeedback);

            // =====================================================
            // REGISTER BUTTON
            // =====================================================

            btnRegister = new Button
            {
                Text = "REGISTER CUSTOMER",
                Location = new Point(480, 218),
                Width = 150,
                Height = 34,
                BackColor = Color.FromArgb(11, 53, 88),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnRegister.Click += async (sender, e) =>
            {
                if (isEditing)
                    await UpdateCustomer();
                else
                    await RegisterCustomer();
            };

            registrationPanel.Controls.Add(btnRegister);

            // =====================================================
            // CANCEL EDIT
            // =====================================================

            btnCancelEdit = new Button
            {
                Text = "CANCEL",
                Location = new Point(640, 218),
                Width = 90,
                Height = 34,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Visible = false,
                Cursor = Cursors.Hand
            };

            btnCancelEdit.Click += (sender, e) =>
            {
                CancelEdit();
            };

            registrationPanel.Controls.Add(btnCancelEdit);

            // =====================================================
            // REFRESH BUTTON
            // =====================================================

            btnRefresh = new Button
            {
                Text = "REFRESH",
                Location = new Point(740, 218),
                Width = 90,
                Height = 34,
                BackColor = Color.Black,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnRefresh.Click += async (sender, e) =>
            {
                await LoadCustomers();
            };

            registrationPanel.Controls.Add(btnRefresh);

            // =====================================================
            // MASTER VIEW
            // =====================================================

            btnMasterView = new Button
            {
                Text = "MASTER VIEW (ALL)",
                Location = new Point(840, 218),
                Width = 140,
                Height = 34,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnMasterView.Click += async (sender, e) =>
            {
                await LoadAllCustomers();
            };

            registrationPanel.Controls.Add(btnMasterView);

            // =====================================================
            // STATUS
            // =====================================================

            lblStatus = new Label
            {
                Text = "Ready",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 355)
            };

            mainPanel.Controls.Add(lblStatus);

            // =====================================================
            // RECORD COUNT
            // =====================================================

            lblRecordCount = new Label
            {
                Text = "Records: 0",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 53, 88),
                AutoSize = true,
                Location = new Point(200, 355)
            };

            mainPanel.Controls.Add(lblRecordCount);

            // =====================================================
            // CUSTOMER RECORDS TITLE
            // =====================================================

            Label lblRecords = new Label
            {
                Text = "CUSTOMER RECORDS",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 53, 88),
                AutoSize = true,
                Location = new Point(20, 385)
            };

            mainPanel.Controls.Add(lblRecords);

            // =====================================================
            // DATA GRID
            // =====================================================

            dgvCustomers = new DataGridView
            {
                Location = new Point(20, 420),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right,

                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,

                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,

                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,

                RowHeadersVisible = false,

                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                RowTemplate = { Height = 38 },

                EnableHeadersVisualStyles = false,

                // Makes the columns share the available horizontal
                // space instead of overflowing and needing a
                // horizontal scrollbar.
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ScrollBars = ScrollBars.Vertical
            };

            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(11, 53, 88);

            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvCustomers.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            dgvCustomers.ColumnHeadersDefaultCellStyle.WrapMode =
                DataGridViewTriState.True;

            dgvCustomers.ColumnHeadersHeight = 48;

            mainPanel.Controls.Add(dgvCustomers);

            CreateGridColumns();
        }

        // =========================================================
        // LABEL HELPER
        // =========================================================

        private void AddLabel(
            Control parent,
            string text,
            int x,
            int y)
        {
            Label label = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true,
                Location = new Point(x, y)
            };

            parent.Controls.Add(label);
        }

        // =========================================================
        // GRID COLUMNS
        // =========================================================

        private void CreateGridColumns()
        {
            // NOTE: Width values below are used as the initial
            // FillWeight ratio since AutoSizeColumnsMode is set to
            // Fill on the grid — the columns will proportionally
            // share whatever horizontal space is available instead
            // of overflowing the visible area.

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                FillWeight = 40,
                MinimumWidth = 40
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                HeaderText = "FULL NAME",
                DataPropertyName = "FullName",
                FillWeight = 110,
                MinimumWidth = 90
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "EMAIL",
                DataPropertyName = "Email",
                FillWeight = 130,
                MinimumWidth = 90
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhoneNumber",
                HeaderText = "PHONE",
                DataPropertyName = "PhoneNumber",
                FillWeight = 90,
                MinimumWidth = 80
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                HeaderText = "ADDRESS",
                DataPropertyName = "Address",
                FillWeight = 130,
                MinimumWidth = 90
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CustomerType",
                HeaderText = "TYPE",
                DataPropertyName = "CustomerType",
                FillWeight = 70,
                MinimumWidth = 60
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FavoriteFood",
                HeaderText = "FAVORITE FOOD",
                DataPropertyName = "FavoriteFood",
                FillWeight = 100,
                MinimumWidth = 80
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "VisitFrequency",
                HeaderText = "VISIT FREQUENCY",
                DataPropertyName = "VisitFrequency",
                FillWeight = 95,
                MinimumWidth = 80
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Feedback",
                HeaderText = "FEEDBACK",
                DataPropertyName = "Feedback",
                FillWeight = 130,
                MinimumWidth = 90
            });

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DateCollected",
                HeaderText = "DATE COLLECTED",
                DataPropertyName = "DateCollected",
                FillWeight = 110,
                MinimumWidth = 90
            });

            // =====================================================
            // EDIT BUTTON
            // =====================================================

            DataGridViewButtonColumn editColumn =
                new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "ACTION",
                    Text = "EDIT",
                    UseColumnTextForButtonValue = true,
                    FillWeight = 55,
                    MinimumWidth = 55
                };

            dgvCustomers.Columns.Add(editColumn);

            // =====================================================
            // ARCHIVE BUTTON
            // =====================================================

            DataGridViewButtonColumn archiveColumn =
                new DataGridViewButtonColumn
                {
                    Name = "Archive",
                    HeaderText = "DELETE",
                    Text = "ARCHIVE",
                    UseColumnTextForButtonValue = true,
                    FillWeight = 65,
                    MinimumWidth = 65
                };

            dgvCustomers.Columns.Add(archiveColumn);

            dgvCustomers.CellContentClick +=
                DgvCustomers_CellContentClick;
        }

        // =========================================================
        // GRID BUTTON ACTIONS
        // =========================================================

        private async void DgvCustomers_CellContentClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex < 0)
                return;

            if (dgvCustomers.Columns[e.ColumnIndex].Name == "Edit")
            {
                EditSelectedCustomer(e.RowIndex);
            }

            else if (dgvCustomers.Columns[e.ColumnIndex].Name == "Archive")
            {
                await ArchiveCustomer(e.RowIndex);
            }
        }

        // =========================================================
        // REGISTER CUSTOMER
        // =========================================================

        private async Task RegisterCustomer()
        {
            if (!ValidateForm())
                return;

            try
            {
                SetStatus("Registering customer...");

                string tenant = cmbTenant.SelectedItem?.ToString() ?? "A";

                CustomerPayload payload = new CustomerPayload
                {
                    FullName = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    PhoneNumber = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    CustomerType =
                        cmbCustomerType.SelectedItem?.ToString() ?? "",
                    FavoriteFood = txtFavoriteFood.Text.Trim(),
                    VisitFrequency =
                        cmbVisitFrequency.SelectedItem?.ToString() ?? "",
                    Feedback = txtFeedback.Text.Trim()
                };

                HttpResponseMessage response =
                    await httpClient.PostAsJsonAsync(
                        $"{ApiBaseUrl}/{tenant}/customers",
                        payload);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Customer registered successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ClearForm();

                    await LoadCustomers();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();

                    MessageBox.Show(
                        "Registration failed.\n\n" + error,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot connect to the API.\n\n" +
                    "Make sure CRM.api is running.\n\n" +
                    ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                SetStatus("API connection failed.");
            }
        }

        // =========================================================
        // LOAD CUSTOMERS
        // =========================================================

        private async Task LoadCustomers()
        {
            try
            {
                string tenant = cmbTenant.SelectedItem?.ToString() ?? "A";

                SetStatus(
                    $"Loading customers from Tenant {tenant}...");

                List<CustomerRow>? customers =
                    await httpClient.GetFromJsonAsync<List<CustomerRow>>(
                        $"{ApiBaseUrl}/{tenant}/customers");

                dgvCustomers.DataSource = customers ??
                                           new List<CustomerRow>();

                lblRecordCount.Text =
                    $"Records: {dgvCustomers.Rows.Count}";

                SetStatus(
                    $"Showing active customers from Tenant {tenant}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load customer records.\n\n" +
                    "Make sure the API is running.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                SetStatus("Failed to load records.");
            }
        }

        // =========================================================
        // MASTER VIEW
        // =========================================================

        private async Task LoadAllCustomers()
        {
            try
            {
                SetStatus("Loading all tenant records...");

                List<TenantGroup>? all =
                    await httpClient.GetFromJsonAsync<List<TenantGroup>>(
                        $"{ApiBaseUrl}/all");

                dgvCustomers.DataSource = null;
                dgvCustomers.Rows.Clear();

                int total = 0;

                if (all != null)
                {
                    foreach (TenantGroup tenant in all)
                    {
                        if (tenant.Customers == null)
                            continue;

                        foreach (CustomerRow customer in tenant.Customers)
                        {
                            int rowIndex =
                                dgvCustomers.Rows.Add();

                            DataGridViewRow row =
                                dgvCustomers.Rows[rowIndex];

                            row.Cells["Id"].Value =
                                customer.Id;

                            row.Cells["FullName"].Value =
                                customer.FullName;

                            row.Cells["Email"].Value =
                                customer.Email;

                            row.Cells["PhoneNumber"].Value =
                                customer.PhoneNumber;

                            row.Cells["Address"].Value =
                                customer.Address;

                            row.Cells["CustomerType"].Value =
                                customer.CustomerType;

                            row.Cells["FavoriteFood"].Value =
                                customer.FavoriteFood;

                            row.Cells["VisitFrequency"].Value =
                                customer.VisitFrequency;

                            row.Cells["Feedback"].Value =
                                customer.Feedback;

                            row.Cells["DateCollected"].Value =
                                customer.DateCollected.ToString(
                                    "yyyy-MM-dd HH:mm");

                            // Master view is for viewing all tenants.
                            row.Cells["Edit"].Value = "VIEW";
                            row.Cells["Archive"].Value = "N/A";

                            total++;
                        }
                    }
                }

                lblRecordCount.Text =
                    $"Records: {total}";

                SetStatus(
                    "Showing customer records from all tenants.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load Master View.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                SetStatus("Failed to load Master View.");
            }
        }

        // =========================================================
        // EDIT CUSTOMER
        // =========================================================

        private void EditSelectedCustomer(int rowIndex)
        {
            if (rowIndex < 0 ||
                rowIndex >= dgvCustomers.Rows.Count)
                return;

            DataGridViewRow row =
                dgvCustomers.Rows[rowIndex];

            try
            {
                selectedCustomerId =
                    Convert.ToInt32(row.Cells["Id"].Value);

                txtName.Text =
                    row.Cells["FullName"].Value?.ToString() ?? "";

                txtEmail.Text =
                    row.Cells["Email"].Value?.ToString() ?? "";

                txtPhone.Text =
                    row.Cells["PhoneNumber"].Value?.ToString() ?? "";

                txtAddress.Text =
                    row.Cells["Address"].Value?.ToString() ?? "";

                txtFavoriteFood.Text =
                    row.Cells["FavoriteFood"].Value?.ToString() ?? "";

                txtFeedback.Text =
                    row.Cells["Feedback"].Value?.ToString() ?? "";

                string customerType =
                    row.Cells["CustomerType"].Value?.ToString() ?? "";

                if (cmbCustomerType.Items.Contains(customerType))
                {
                    cmbCustomerType.SelectedItem =
                        customerType;
                }

                string visitFrequency =
                    row.Cells["VisitFrequency"].Value?.ToString() ?? "";

                if (cmbVisitFrequency.Items.Contains(
                    visitFrequency))
                {
                    cmbVisitFrequency.SelectedItem =
                        visitFrequency;
                }

                isEditing = true;

                btnRegister.Text = "SAVE CHANGES";
                btnRegister.BackColor =
                    Color.FromArgb(0, 120, 90);

                btnCancelEdit.Visible = true;

                SetStatus(
                    $"Editing customer ID {selectedCustomerId}.");

                txtName.Focus();
            }
            catch
            {
                MessageBox.Show(
                    "Unable to edit this customer record.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // UPDATE CUSTOMER
        // =========================================================

        private async Task UpdateCustomer()
        {
            if (!ValidateForm())
                return;

            if (selectedCustomerId <= 0)
            {
                MessageBox.Show(
                    "No customer selected for update.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                string tenant =
                    cmbTenant.SelectedItem?.ToString() ?? "A";

                CustomerPayload payload =
                    new CustomerPayload
                    {
                        FullName = txtName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        PhoneNumber = txtPhone.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        CustomerType =
                            cmbCustomerType.SelectedItem?.ToString() ?? "",
                        FavoriteFood =
                            txtFavoriteFood.Text.Trim(),
                        VisitFrequency =
                            cmbVisitFrequency.SelectedItem?.ToString() ?? "",
                        Feedback =
                            txtFeedback.Text.Trim()
                    };

                SetStatus("Updating customer...");

                HttpResponseMessage response =
                    await httpClient.PutAsJsonAsync(
                        $"{ApiBaseUrl}/{tenant}/customers/{selectedCustomerId}",
                        payload);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Customer updated successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    CancelEdit();

                    await LoadCustomers();
                }
                else
                {
                    string error =
                        await response.Content.ReadAsStringAsync();

                    MessageBox.Show(
                        "Update failed.\n\n" + error,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update customer.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // ARCHIVE CUSTOMER
        // =========================================================

        private async Task ArchiveCustomer(int rowIndex)
        {
            if (rowIndex < 0 ||
                rowIndex >= dgvCustomers.Rows.Count)
                return;

            DataGridViewRow row =
                dgvCustomers.Rows[rowIndex];

            int customerId;

            try
            {
                customerId =
                    Convert.ToInt32(row.Cells["Id"].Value);
            }
            catch
            {
                return;
            }

            string customerName =
                row.Cells["FullName"].Value?.ToString() ?? "";

            DialogResult result =
                MessageBox.Show(
                    $"Are you sure you want to archive:\n\n" +
                    $"{customerName}?\n\n" +
                    "The record will be removed from the active list " +
                    "but will remain stored in the database.",
                    "Archive Customer",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                string tenant =
                    cmbTenant.SelectedItem?.ToString() ?? "A";

                SetStatus("Archiving customer...");

                HttpResponseMessage response =
                    await httpClient.DeleteAsync(
                        $"{ApiBaseUrl}/{tenant}/customers/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Customer archived successfully.",
                        "Archived",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    await LoadCustomers();
                }
                else
                {
                    string error =
                        await response.Content.ReadAsStringAsync();

                    MessageBox.Show(
                        "Archive failed.\n\n" + error,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to archive customer.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // VALIDATION
        // =========================================================

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowValidation(
                    "Full Name is required.",
                    txtName);

                return false;
            }

            // EMAIL IS OPTIONAL.
            // No email validation is required.

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                ShowValidation(
                    "Phone Number is required.",
                    txtPhone);

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                ShowValidation(
                    "Address is required.",
                    txtAddress);

                return false;
            }

            if (cmbCustomerType.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select Customer Type.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCustomerType.Focus();

                return false;
            }

            if (cmbVisitFrequency.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select Visit Frequency.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbVisitFrequency.Focus();

                return false;
            }

            return true;
        }

        // =========================================================
        // VALIDATION MESSAGE
        // =========================================================

        private void ShowValidation(
            string message,
            Control control)
        {
            MessageBox.Show(
                message,
                "Validation Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            control.Focus();
        }

        // =========================================================
        // CANCEL EDIT
        // =========================================================

        private void CancelEdit()
        {
            isEditing = false;
            selectedCustomerId = 0;

            btnRegister.Text = "REGISTER CUSTOMER";

            btnRegister.BackColor =
                Color.FromArgb(11, 53, 88);

            btnCancelEdit.Visible = false;

            ClearForm();

            SetStatus("Ready for new customer registration.");
        }

        // =========================================================
        // CLEAR FORM
        // =========================================================

        private void ClearForm()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtFavoriteFood.Clear();
            txtFeedback.Clear();

            cmbCustomerType.SelectedIndex = -1;
            cmbVisitFrequency.SelectedIndex = -1;

            txtName.Focus();
        }

        // =========================================================
        // STATUS
        // =========================================================

        private void SetStatus(string message)
        {
            lblStatus.Text = message;
        }
    }

    // =============================================================
    // CUSTOMER PAYLOAD
    // =============================================================

    public class CustomerPayload
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string CustomerType { get; set; } = "";
        public string FavoriteFood { get; set; } = "";
        public string VisitFrequency { get; set; } = "";
        public string Feedback { get; set; } = "";
    }

    // =============================================================
    // CUSTOMER ROW
    // =============================================================

    public class CustomerRow
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string PhoneNumber { get; set; } = "";

        public string Address { get; set; } = "";

        public string CustomerType { get; set; } = "";

        public string FavoriteFood { get; set; } = "";

        public string VisitFrequency { get; set; } = "";

        public string Feedback { get; set; } = "";

        public DateTime DateCollected { get; set; }
    }

    // =============================================================
    // MASTER TENANT GROUP
    // =============================================================

    public class TenantGroup
    {
        public string TenantCode { get; set; } = "";

        public string TenantName { get; set; } = "";

        public List<CustomerRow> Customers { get; set; } =
            new List<CustomerRow>();
    }
}