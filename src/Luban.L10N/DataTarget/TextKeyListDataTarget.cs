using System.Text;
using Luban.DataTarget;
using Luban.DataVisitors;
using Luban.Defs;
using NLog;

namespace Luban.L10N.DataTarget;

[DataTarget("text-list")]
internal class TextKeyListDataTarget : DataTargetBase
{
    protected override string OutputFileExt => "txt";

    public override bool ExportAllRecords => true;

    public override AggregationType AggregationType => AggregationType.Tables;

    public override OutputFile ExportTable(DefTable table, List<Record> records)
    {
        throw new NotImplementedException();
    }

    public override OutputFile ExportTables(List<DefTable> tables)
    {
        var textCollection = new TextKeyCollection();

        var visitor = new DataActionHelpVisitor2<TextKeyCollection>(TextKeyListCollectorVisitor.Ins);

        foreach (var table in tables)
        {
            TableVisitor.Ins.Visit(table, visitor, textCollection);
        }

        var keys = textCollection.Keys.ToList();
        keys.Sort((a, b) => string.Compare(a, b, StringComparison.Ordinal));
        string content = null;

        var sLogger = NLog.LogManager.GetCurrentClassLogger();
        string outputFile = EnvManager.Current.GetOption(BuiltinOptionNames.L10NFamily, BuiltinOptionNames.TextKeyListFile, false);
        string outputDir = EnvManager.Current.GetOption($"{outputFile}", BuiltinOptionNames.OutputDataDir, true);
        string fullOutputPath = $"{outputDir}/{outputFile}";
        sLogger.Info($"language outputFile:{outputFile} \n fullPath:{fullOutputPath}");
        if (File.Exists(fullOutputPath))
        {
            sLogger.Warn("language file exist! check and replace ---");
            LanguageManager language = new LanguageManager();
            language.Load(File.ReadAllText(fullOutputPath));
            StringBuilder builder = new StringBuilder(keys.Count * 6);
            foreach (var key in keys)
            {
                builder.Append(key);
                if (language.Has(key))
                {
                    builder.Append('=');
                    builder.Append(language.Get(key));
                }

                builder.Append('\n');
            }

            content = builder.ToString();
        }
        else content = string.Join("\n", keys);

        return new OutputFile { File = outputFile, Content = content };
    }
}
