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
        public static List<IRule> ReadPresetFile(OpenFileDialog target, RuleFactory ruleFactory, out bool success)
        {
            success = false;
            string fileName = target.FileName;
            if (fileName == "") return new List<IRule>();

            string extension = Path.GetExtension(fileName).ToLower();
            if (extension != "") extension = extension.Substring(1);

            List<IRule> newRuleList = new List<IRule>();
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
                                newRuleList.Add(ruleFactory.StringToIRuleConverter(rawRule));
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
                        newRuleList.Add(ruleFactory.StringToIRuleConverter(line));
                    }
                    break;
                
                default:
                    MessageBox.Show($"Unsupported file type ({extension})", "Rule Preset Reader", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
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
