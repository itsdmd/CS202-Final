using Contract;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace FinalProject
{
    public static class PresetProcessor
    {
        public const int NOT_FOUND_LIMIT = 8;

        public static List<IRule> ReadPresetFile(OpenFileDialog target, RuleFactory ruleFactory, out bool success)
        {
            success = false;
            string fileName = target.FileName;
            if (fileName == "") return new List<IRule>();

            string extension = Path.GetExtension(fileName).ToLower();
            if (extension != "") extension = extension.Substring(1);

            List<IRule> newRuleList = new List<IRule>();
            List<string> notFoundList = new List<string>();
            // Load rule preset files with the following file types:
            switch (extension)
            {
                // JSON RULE PRESET FILE
                case "json":
                    try
                    {
                        string rawJson = File.ReadAllText(fileName);
                        List<RuleDeserializeTemplate>? deserializedRules = JsonSerializer.Deserialize<List<RuleDeserializeTemplate>>(rawJson);

                        if (deserializedRules != null)
                        {
                            //newRuleList = ruleList;

                            foreach (RuleDeserializeTemplate ruleTemplate in deserializedRules)
                            {
                                string rawRule = $"{ruleTemplate.Name} {ruleTemplate.Config}";
                                Debug.WriteLine(rawRule);
                                IRule? rule = ruleFactory.StringToIRuleConverter(rawRule, out bool found);
                                if (rule != null)
                                    newRuleList.Add(rule);

                                if (!found || rule == null)
                                    if (!notFoundList.Contains(ruleTemplate.Name))
                                        notFoundList.Add(ruleTemplate.Name);
                            }

                            success = true;
                        }
                        /*
                        else
                        {
                            MessageBox.Show("Deserialized list is null.");
                        }
                        */
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Unable to deserialize JSON Preset.\n\n{e.Message}", e.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;

                // RAW RULE PRESET FILE
                case "txt":
                    List<string> fileContent = new List<string>();
                    try
                    {
                        fileContent = new List<string>(File.ReadAllLines(fileName));
                        success = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Rule file not found", e.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    foreach (string line in fileContent)
                    {
                        IRule? rule = ruleFactory.StringToIRuleConverter(line, out bool found);
                        if (rule != null)
                            newRuleList.Add(rule);

                        if (!found || rule == null)
                            if (!notFoundList.Contains(line.Split(' ')[0]))
                                notFoundList.Add(line.Split(' ')[0]);
                    }
                    break;

                default:
                    MessageBox.Show($"Unsupported file type ({extension})", "Rule Preset Reader", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }

            if (notFoundList.Count > 0)
            {
                string overflowNotice = "";
                int overflow = notFoundList.Count - NOT_FOUND_LIMIT;
                if (overflow > 0)
                {
                    overflowNotice = $"\nAnd {overflow} more...";
                    notFoundList.RemoveRange(NOT_FOUND_LIMIT - 1, overflow);
                }
                
                MessageBox.Show($"Unable to load the following rules:\n\n- {string.Join("\n- ", notFoundList)}{overflowNotice}", "StringToIRuleConverter", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            return newRuleList;
        }

        public static bool WritePresetFile(SaveFileDialog target, RuleFactory ruleFactory)
        {
            string fileName = target.FileName;
            if (fileName == "") return false;

            string extension = Path.GetExtension(fileName).ToLower();
            if (extension != "") extension = extension.Substring(1);

            string result = "";
            // Save rule preset file in the following file types:
            switch (extension)
            {
                case "json":
                    result = ruleFactory.IRuleToStringConverter(true);
                    break;

                default:
                case "txt":
                    result = ruleFactory.IRuleToStringConverter();
                    break;
            }

            byte[] buffer = new UTF8Encoding().GetBytes(result);

            bool success = true;
            using (FileStream fs = File.Create(fileName, buffer.Length))
            {
                try
                {
                    fs.Write(buffer, 0, buffer.Length);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"An error has occured while saving Rule Preset File.\nTarget Path: {fileName}\nException: {e}\nDetails: {e.Message}", e.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    success = false;
                }
                finally
                {
                    fs.Close();
                }

            };
            return success;
        }
    }
}
