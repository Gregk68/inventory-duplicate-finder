using System.Data;
using InventoryDuplicateFinder.Models;
using InventoryDuplicateFinder.Services;

namespace InventoryDuplicateFinder;

public partial class Form1 : Form
{
    private const int HorizontalMargin = 12;
    private const int TopGridY = 115;
    private const int BottomMargin = 12;
    private const int SectionGap = 8;
    private const int LabelToGridGap = 2;
    private const int SplitterThickness = 6;
    private const int MinDuplicatesHeight = 140;
    private const int MinIgnoredHeight = 54;
    private const int MinWarningsHeight = 72;
    private const string AppVersion = "1.1.15";

    private readonly InventoryParser _parser = new();
    private readonly DuplicateAnalyzer _analyzer = new();
    private readonly UserSettings _settings = UserSettings.Default;
    private readonly HashSet<int> _ignoredIds = [];
    private readonly HashSet<string> _exaltationInitializedFiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly Panel _splitterTop = new();
    private readonly Panel _splitterBottom = new();
    private readonly ToolTip _copyToolTip = new();

    private bool _dragTopSplitter;
    private bool _dragBottomSplitter;
    private int _dragStartY;
    private int _dragStartIgnoredHeight;
    private int _dragStartWarningsHeight;

    private int? _userIgnoredHeight;
    private int? _userWarningsHeight;

    private ParseResult? _lastParseResult;

    public Form1()
    {
        InitializeComponent();
        LoadSavedSettings();
        ConfigureSplitters();
        ConfigureGridBehavior();
        ConfigureCopyToolTip();
        Resize += Form1_Resize;
        RecalculateTableLayout();
    }

    private void ConfigureCopyToolTip()
    {
        _copyToolTip.AutomaticDelay = 80;
        _copyToolTip.ReshowDelay = 0;
        _copyToolTip.InitialDelay = 0;
        _copyToolTip.AutoPopDelay = 1600;
        _copyToolTip.ShowAlways = true;
        _copyToolTip.IsBalloon = true;
        _copyToolTip.UseAnimation = true;
        _copyToolTip.UseFading = true;
        _copyToolTip.ToolTipTitle = "Copied";
    }

    private void ConfigureSplitters()
    {
        SetupSplitterPanel(_splitterTop);
        SetupSplitterPanel(_splitterBottom);

        _splitterTop.MouseDown += TopSplitter_MouseDown;
        _splitterBottom.MouseDown += BottomSplitter_MouseDown;

        _splitterTop.MouseMove += Splitter_MouseMove;
        _splitterBottom.MouseMove += Splitter_MouseMove;

        _splitterTop.MouseUp += Splitter_MouseUp;
        _splitterBottom.MouseUp += Splitter_MouseUp;

        Controls.Add(_splitterTop);
        Controls.Add(_splitterBottom);

        _splitterTop.BringToFront();
        _splitterBottom.BringToFront();
    }

    private static void SetupSplitterPanel(Panel splitter)
    {
        splitter.Height = SplitterThickness;
        splitter.BackColor = SystemColors.ControlDark;
        splitter.Cursor = Cursors.HSplit;
        splitter.TabStop = false;
    }

    private void TopSplitter_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        _dragTopSplitter = true;
        _dragStartY = Cursor.Position.Y;
        _dragStartIgnoredHeight = dgvIgnored.Height;
    }

    private void BottomSplitter_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        _dragBottomSplitter = true;
        _dragStartY = Cursor.Position.Y;
        _dragStartIgnoredHeight = dgvIgnored.Height;
        _dragStartWarningsHeight = dgvWarnings.Height;
    }

    private void Splitter_MouseMove(object? sender, MouseEventArgs e)
    {
        int delta = Cursor.Position.Y - _dragStartY;

        if (_dragTopSplitter)
        {
            int nextIgnored = _dragStartIgnoredHeight - delta;
            _userIgnoredHeight = Math.Max(MinIgnoredHeight, nextIgnored);
            RecalculateTableLayout();
        }
        else if (_dragBottomSplitter)
        {
            int nextIgnored = _dragStartIgnoredHeight + delta;
            int nextWarnings = _dragStartWarningsHeight - delta;

            _userIgnoredHeight = Math.Max(MinIgnoredHeight, nextIgnored);
            _userWarningsHeight = Math.Max(MinWarningsHeight, nextWarnings);
            RecalculateTableLayout();
        }
    }

    private void Splitter_MouseUp(object? sender, MouseEventArgs e)
    {
        _dragTopSplitter = false;
        _dragBottomSplitter = false;
    }

    private void ConfigureGridBehavior()
    {
        ConfigureGrid(dgvDuplicates);
        ConfigureGrid(dgvIgnored);
        ConfigureGrid(dgvWarnings);
    }

    private static void ConfigureGrid(DataGridView grid)
    {
        grid.AllowUserToResizeColumns = true;
        grid.AllowUserToResizeRows = true;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }

    private void Form1_Resize(object? sender, EventArgs e)
    {
        RecalculateTableLayout();
    }

    private void RecalculateTableLayout()
    {
        int availableWidth = Math.Max(100, ClientSize.Width - (HorizontalMargin * 2));

        int labelsHeight = lblIgnoredItems.Height + lblWarnings.Height;
        int spacingTotal = (SectionGap * 3) + (LabelToGridGap * 2);
        int totalGridHeight = ClientSize.Height - TopGridY - BottomMargin - labelsHeight - spacingTotal;
        totalGridHeight = Math.Max(MinDuplicatesHeight + MinIgnoredHeight + MinWarningsHeight, totalGridHeight);

        int maxIgnoredAllowed = Math.Max(MinIgnoredHeight, totalGridHeight - MinDuplicatesHeight - MinWarningsHeight);
        int maxWarningsAllowed = Math.Max(MinWarningsHeight, totalGridHeight - MinDuplicatesHeight - MinIgnoredHeight);

        int ignoredHeight = _userIgnoredHeight ?? Math.Max(MinIgnoredHeight, (int)(totalGridHeight * 0.15));
        int warningsHeight = _userWarningsHeight ?? Math.Max(MinWarningsHeight, (int)(totalGridHeight * 0.2));

        ignoredHeight = Math.Max(MinIgnoredHeight, Math.Min(maxIgnoredAllowed, ignoredHeight));
        warningsHeight = Math.Max(MinWarningsHeight, Math.Min(maxWarningsAllowed, warningsHeight));

        int duplicatesHeight = totalGridHeight - ignoredHeight - warningsHeight;

        if (duplicatesHeight < MinDuplicatesHeight)
        {
            int deficit = MinDuplicatesHeight - duplicatesHeight;

            int reduceWarnings = Math.Min(deficit, warningsHeight - MinWarningsHeight);
            warningsHeight -= reduceWarnings;
            deficit -= reduceWarnings;

            int reduceIgnored = Math.Min(deficit, ignoredHeight - MinIgnoredHeight);
            ignoredHeight -= reduceIgnored;

            duplicatesHeight = totalGridHeight - ignoredHeight - warningsHeight;
        }

        _userIgnoredHeight = ignoredHeight;
        _userWarningsHeight = warningsHeight;

        int x = HorizontalMargin;
        int y = TopGridY;

        dgvDuplicates.SetBounds(x, y, availableWidth, duplicatesHeight);
        y = dgvDuplicates.Bottom + SectionGap;

        _splitterTop.SetBounds(x, y, availableWidth, SplitterThickness);
        y = _splitterTop.Bottom + SectionGap;

        lblIgnoredItems.Location = new Point(x, y);
        y = lblIgnoredItems.Bottom + LabelToGridGap;

        dgvIgnored.SetBounds(x, y, availableWidth, ignoredHeight);
        y = dgvIgnored.Bottom + SectionGap;

        _splitterBottom.SetBounds(x, y, availableWidth, SplitterThickness);
        y = _splitterBottom.Bottom + SectionGap;

        lblWarnings.Location = new Point(x, y);
        y = lblWarnings.Bottom + LabelToGridGap;

        int warningsFinalHeight = Math.Max(MinWarningsHeight, ClientSize.Height - BottomMargin - y);
        dgvWarnings.SetBounds(x, y, availableWidth, warningsFinalHeight);

        RedrawTablesForNewSize();
    }

    private void RedrawTablesForNewSize()
    {
        RedrawTable(dgvDuplicates);
        RedrawTable(dgvIgnored);
        RedrawTable(dgvWarnings);
    }

    private static void RedrawTable(DataGridView grid)
    {
        if (grid.IsHandleCreated)
        {
            grid.AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders);
            grid.Refresh();
        }
    }

    private void LoadSavedSettings()
    {
        _ignoredIds.Clear();
        foreach (int id in ParseIgnoredIds(_settings.IgnoredIds))
        {
            _ignoredIds.Add(id);
        }
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "Inventory files (*-Inventory.txt)|*-Inventory.txt|Text files (*.txt)|*.txt|All files (*.*)|*.*";
        openFileDialog.Title = "Select inventory file";

        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        {
            txtFilePath.Text = openFileDialog.FileName;
        }
    }

    private void btnAnalyze_Click(object sender, EventArgs e)
    {
        string filePath = txtFilePath.Text.Trim();
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            MessageBox.Show(this, "Please select a valid inventory file.", "File Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _lastParseResult = _parser.Parse(filePath);
        ApplyDefaultExaltationIgnore(filePath, _lastParseResult);
        RefreshTables();
    }

    private void btnAbout_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            this,
            $"Version: {AppVersion}\n\nMade by\nGazz  -  Oggok",
            "About",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void ApplyDefaultExaltationIgnore(string filePath, ParseResult parseResult)
    {
        if (_exaltationInitializedFiles.Contains(filePath))
        {
            return;
        }

        IEnumerable<int> exaltationIds = parseResult.Rows
            .Where(r => r.Id != 0 && r.Name.Contains("(Exaltation)", StringComparison.OrdinalIgnoreCase))
            .Select(r => r.Id)
            .Distinct();

        foreach (int id in exaltationIds)
        {
            _ignoredIds.Add(id);
        }

        _exaltationInitializedFiles.Add(filePath);
        PersistIgnoredIds();
    }

    private void btnToggleExaltation_Click(object sender, EventArgs e)
    {
        string filePath = txtFilePath.Text.Trim();
        if (_lastParseResult is null)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show(this, "Please select a valid inventory file first.", "File Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _lastParseResult = _parser.Parse(filePath);
        }

        HashSet<int> exaltationIds = _lastParseResult.Rows
            .Where(r => r.Id != 0 && r.Name.Contains("(Exaltation)", StringComparison.OrdinalIgnoreCase))
            .Select(r => r.Id)
            .ToHashSet();

        if (exaltationIds.Count == 0)
        {
            MessageBox.Show(this, "No Exaltation items were found in this file.", "No Items Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        bool allAlreadyIgnored = exaltationIds.All(id => _ignoredIds.Contains(id));
        if (allAlreadyIgnored)
        {
            foreach (int id in exaltationIds)
            {
                _ignoredIds.Remove(id);
            }
        }
        else
        {
            foreach (int id in exaltationIds)
            {
                _ignoredIds.Add(id);
            }
        }

        PersistIgnoredIds();
        RefreshTables();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        _lastParseResult = null;
        dgvDuplicates.DataSource = null;
        dgvIgnored.DataSource = null;
        dgvWarnings.DataSource = null;
        lblSummary.Text = "Ready";
    }

    private void RefreshTables()
    {
        if (_lastParseResult is null)
        {
            dgvDuplicates.DataSource = null;
            dgvIgnored.DataSource = null;
            dgvWarnings.DataSource = null;
            lblSummary.Text = "Ready";
            return;
        }

        List<DuplicateGroup> duplicates = _analyzer.FindDuplicateIds(_lastParseResult.Rows, _ignoredIds, excludeZeroIds: true);

        dgvDuplicates.DataSource = BuildResultsTable(duplicates);
        ApplyDuplicatesColumnSizing();

        dgvIgnored.DataSource = BuildIgnoredTable(_lastParseResult.Rows, _ignoredIds);
        ApplyIgnoredColumnSizing();

        dgvWarnings.DataSource = BuildWarningsTable(_lastParseResult.Warnings);
        ApplyWarningsColumnSizing();
        dgvWarnings.DefaultCellStyle.BackColor = Color.LightYellow;

        lblSummary.Text = $"Rows: {_lastParseResult.Rows.Count}  |  Warnings: {_lastParseResult.WarningCount}  |  Duplicates: {duplicates.Count}  |  Ignored IDs: {_ignoredIds.Count}";
    }

    private void ApplyDuplicatesColumnSizing()
    {
        if (dgvDuplicates.Columns.Count == 0)
        {
            return;
        }

        dgvDuplicates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

        DataGridViewColumn toggle = dgvDuplicates.Columns["Toggle"];
        DataGridViewColumn occurrences = dgvDuplicates.Columns["Occurances"];
        DataGridViewColumn id = dgvDuplicates.Columns["ID"];
        DataGridViewColumn name = dgvDuplicates.Columns["Name"];
        DataGridViewColumn location = dgvDuplicates.Columns["Location"];

        ConfigureMinimalColumn(toggle);
        ConfigureMinimalColumn(occurrences);
        ConfigureMinimalColumn(id);

        ApplyDynamicNameColumnSizing(dgvDuplicates, name);
        ConfigureFillColumn(location, 50);
    }

    private void ApplyIgnoredColumnSizing()
    {
        if (dgvIgnored.Columns.Count == 0)
        {
            return;
        }

        dgvIgnored.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

        DataGridViewColumn toggle = dgvIgnored.Columns["Toggle"];
        DataGridViewColumn id = dgvIgnored.Columns["ID"];
        DataGridViewColumn occurrences = dgvIgnored.Columns["Occurances"];
        DataGridViewColumn name = dgvIgnored.Columns["Name"];
        DataGridViewColumn location = dgvIgnored.Columns["Location"];

        ConfigureMinimalColumn(toggle);
        ConfigureMinimalColumn(occurrences);
        ConfigureMinimalColumn(id);

        ApplyDynamicNameColumnSizing(dgvIgnored, name);
        ConfigureFillColumn(location, 50);
    }

    private static void ApplyDynamicNameColumnSizing(DataGridView grid, DataGridViewColumn nameColumn)
    {
        nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        nameColumn.MinimumWidth = 80;

        int preferredWidth = nameColumn.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);

        int desiredWidth = Math.Max(nameColumn.MinimumWidth, preferredWidth + 8);
        nameColumn.Width = Math.Min(desiredWidth, grid.Width / 2);
    }

    private void ApplyWarningsColumnSizing()
    {
        if (dgvWarnings.Columns.Count == 0)
        {
            return;
        }

        dgvWarnings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

        DataGridViewColumn line = dgvWarnings.Columns["Line"];
        DataGridViewColumn warning = dgvWarnings.Columns["Warning"];
        DataGridViewColumn rawLine = dgvWarnings.Columns["Raw Line"];

        ConfigureMinimalColumn(line);
        ConfigureFillColumn(warning, 40);
        ConfigureFillColumn(rawLine, 60);
    }

    private static void ConfigureMinimalColumn(DataGridViewColumn column)
    {
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        column.MinimumWidth = 40;
    }

    private static void ConfigureFillColumn(DataGridViewColumn column, int fillWeight)
    {
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        column.FillWeight = fillWeight;
        column.MinimumWidth = 80;
    }

    private void PersistIgnoredIds()
    {
        _settings.IgnoredIds = string.Join(", ", _ignoredIds.OrderBy(i => i));
        _settings.Save();
    }

    private static HashSet<int> ParseIgnoredIds(string raw)
    {
        HashSet<int> ids = [];
        string[] parts = raw.Split([',', ';', '\n', '\r', '\t', ' '], StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            if (int.TryParse(part.Trim(), out int id))
            {
                ids.Add(id);
            }
        }

        return ids;
    }

    private void dgvDuplicates_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (_lastParseResult is null || e.RowIndex < 0)
        {
            return;
        }

        if (e.ColumnIndex == dgvDuplicates.Columns["Name"].Index)
        {
            object? nameValue = dgvDuplicates.Rows[e.RowIndex].Cells["Name"].Value;
            string? nameText = nameValue?.ToString();
            string copiedName = ExtractPrimaryName(nameText);
            if (!string.IsNullOrWhiteSpace(copiedName))
            {
                Clipboard.SetText(copiedName);
                ShowCopiedTooltip();
            }
            return;
        }

        if (e.ColumnIndex != dgvDuplicates.Columns["Toggle"].Index)
        {
            return;
        }

        object? idValue = dgvDuplicates.Rows[e.RowIndex].Cells["ID"].Value;
        if (idValue is null)
        {
            return;
        }

        if (int.TryParse(idValue.ToString(), out int id))
        {
            ToggleIgnoredId(id);
        }
    }

    private static DataTable BuildResultsTable(IEnumerable<DuplicateGroup> duplicates)
    {
        DataTable table = new();
        table.Columns.Add("Toggle", typeof(string));
        table.Columns.Add("Occurances", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Location", typeof(string));
        table.Columns.Add("ID", typeof(int));

        foreach (DuplicateGroup duplicate in duplicates)
        {
            table.Rows.Add(
                "Ignore",
                duplicate.Occurrences,
                string.Join(" | ", duplicate.Names),
                string.Join(" | ", duplicate.Locations),
                duplicate.Id);
        }

        return table;
    }

    private void dgvIgnored_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (_lastParseResult is null || e.RowIndex < 0)
        {
            return;
        }

        if (e.ColumnIndex == dgvIgnored.Columns["Name"].Index)
        {
            object? nameValue = dgvIgnored.Rows[e.RowIndex].Cells["Name"].Value;
            string? nameText = nameValue?.ToString();
            string copiedName = ExtractPrimaryName(nameText);
            if (!string.IsNullOrWhiteSpace(copiedName))
            {
                Clipboard.SetText(copiedName);
                ShowCopiedTooltip();
            }
            return;
        }

        if (e.ColumnIndex != dgvIgnored.Columns["Toggle"].Index)
        {
            return;
        }

        object? idValue = dgvIgnored.Rows[e.RowIndex].Cells["ID"].Value;
        if (idValue is null)
        {
            return;
        }

        if (int.TryParse(idValue.ToString(), out int id))
        {
            ToggleIgnoredId(id);
        }
    }

    private void ToggleIgnoredId(int id)
    {
        if (_ignoredIds.Contains(id))
        {
            _ignoredIds.Remove(id);
        }
        else
        {
            _ignoredIds.Add(id);
        }

        PersistIgnoredIds();
        RefreshTables();
    }

    private void ShowCopiedTooltip()
    {
        Point cursor = PointToClient(Cursor.Position);
        int tipX = Math.Max(10, Math.Min(ClientSize.Width - 180, cursor.X + 14));
        int tipY = Math.Max(10, Math.Min(ClientSize.Height - 60, cursor.Y + 14));

        _copyToolTip.Hide(this);
        _copyToolTip.Show("Copied to clipboard", this, tipX, tipY, 1600);
    }

    private static string ExtractPrimaryName(string? rawName)
    {
        if (string.IsNullOrWhiteSpace(rawName))
        {
            return string.Empty;
        }

        int pipeIndex = rawName.IndexOf('|');
        string beforePipe = pipeIndex >= 0 ? rawName[..pipeIndex] : rawName;
        string cleaned = beforePipe.TrimEnd();

        int plusIndex = cleaned.LastIndexOf('+');
        if (plusIndex > 0 && cleaned[plusIndex - 1] == ' ')
        {
            string suffix = cleaned[(plusIndex + 1)..];
            if (suffix.Length > 0 && suffix.All(char.IsDigit))
            {
                cleaned = cleaned[..plusIndex].TrimEnd();
            }
        }

        return cleaned;
    }

    private static DataTable BuildIgnoredTable(IEnumerable<InventoryRow> rows, HashSet<int> ignoredIds)
    {
        DataTable table = new();
        table.Columns.Add("Toggle", typeof(string));
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Occurances", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Location", typeof(string));

        IEnumerable<IGrouping<int, InventoryRow>> groups = rows
            .Where(r => ignoredIds.Contains(r.Id))
            .GroupBy(r => r.Id)
            .OrderBy(g => g.Key);

        foreach (IGrouping<int, InventoryRow> group in groups)
        {
            table.Rows.Add(
                "Unignore",
                group.Key,
                group.Count(),
                string.Join(" | ", group.Select(r => r.Name).Where(n => !string.IsNullOrWhiteSpace(n)).Distinct().OrderBy(n => n)),
                string.Join(" | ", group.Select(r => r.Location).Where(l => !string.IsNullOrWhiteSpace(l)).Distinct().OrderBy(l => l)));
        }

        return table;
    }

    private static DataTable BuildWarningsTable(IEnumerable<ParseWarning> warnings)
    {
        DataTable table = new();
        table.Columns.Add("Line", typeof(int));
        table.Columns.Add("Warning", typeof(string));
        table.Columns.Add("Raw Line", typeof(string));

        foreach (ParseWarning warning in warnings.OrderBy(w => w.LineNumber))
        {
            table.Rows.Add(warning.LineNumber, warning.Message, warning.RawLine);
        }

        return table;
    }
}
