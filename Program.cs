using System;
using System.IO;
using System.Linq;

class PassGen
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Password Generator";
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("===================================");
        Console.WriteLine("      Генератор паролей");
        Console.WriteLine("===================================");

        string lastPassword = LoadLastPassword();
        if (!string.IsNullOrWhiteSpace(lastPassword))
        {
            Console.WriteLine("Последний сгенерированный пароль:");
            Console.WriteLine(lastPassword);
            Console.WriteLine();
        }

        Console.WriteLine("Выберите язык:");
        Console.WriteLine("1. Русский");
        Console.WriteLine("2. English");

        int languageChoice;
        if (!int.TryParse(Console.ReadLine(), out languageChoice) || languageChoice < 1 || languageChoice > 2)
        {
            Console.WriteLine("Неверный выбор языка. Завершение.");
            return;
        }

        bool useEnglish = (languageChoice == 1);

        string languagePrompt = useEnglish ? "Сколько символов должно быть в пароле:" : "How many characters should the password have:";
        string includeNumbersPrompt = useEnglish ? "Включить цифры в пароле? (да/нет):" : "Include numbers in the password? (yes/no):";
        string includeUppercasePrompt = useEnglish ? "Включить заглавные буквы в пароле? (да/нет):" : "Include uppercase letters in the password? (yes/no):";

        Console.WriteLine(languagePrompt);

        int passwordLength;
        if (!int.TryParse(Console.ReadLine(), out passwordLength) || passwordLength < 1)
        {
            Console.WriteLine("Неверная длина пароля. Завершение.");
            return;
        }

        Console.WriteLine(includeNumbersPrompt);
        bool includeNumbers = Console.ReadLine().Trim().Equals(useEnglish ? "да" : "yes", StringComparison.OrdinalIgnoreCase);

        Console.WriteLine(includeUppercasePrompt);
        bool includeUppercase = Console.ReadLine().Trim().Equals(useEnglish ? "да" : "yes", StringComparison.OrdinalIgnoreCase);

        Console.WriteLine(useEnglish ? "Включить ключевое слово в пароль? (Введите ключевое слово или 'нет' для пропуска):" : "Include a keyword in the password? (Type the keyword or 'no' to skip):");
        string keyword = Console.ReadLine().Trim();
        bool useKeyword = !string.Equals(keyword, useEnglish ? "нет" : "no", StringComparison.OrdinalIgnoreCase);

        string keywordLocation = "anywhere";
        if (useKeyword)
        {
            Console.WriteLine(useEnglish ? "Где вы хотите разместить ключевое слово в пароле? (1 для начала, 2 для конца, 3 для произвольного места):" : "Where would you like the keyword in the password? (1 for beginning, 2 for end, 3 for anywhere):");

            int keywordLocationChoice;
            if (!int.TryParse(Console.ReadLine(), out keywordLocationChoice) || keywordLocationChoice < 1 || keywordLocationChoice > 3)
            {
                Console.WriteLine("Неверный выбор расположения ключевого слова. Завершение.");
                return;
            }

            switch (keywordLocationChoice)
            {
                case 1:
                    keywordLocation = "beginning";
                    break;
                case 2:
                    keywordLocation = "end";
                    break;
                case 3:
                    keywordLocation = "anywhere";
                    break;
            }
        }

        string password = GeneratePassword(passwordLength, includeNumbers, includeUppercase, useKeyword, keyword, keywordLocation);

        Console.WriteLine("===================================");
        Console.WriteLine(useEnglish ? "Сгенерированный пароль: " + password : "Generated password: " + password);
        Console.WriteLine("===================================");

        bool savePassword = AskToSavePassword(useEnglish);
        if (savePassword)
        {
            SaveLastPassword(password);
            Console.WriteLine("Пароль сохранен.");
        }

        CheckPasswordComplexity(password, useEnglish);

        string thankYouMessage = useEnglish ? "Спасибо за использование Генератора паролей!" : "Thank you for using the Password Generator!";
        Console.WriteLine(thankYouMessage);

        Console.WriteLine("===================================");
        Console.WriteLine(useEnglish ? "Our Social Media Accounts:" : "Наши аккаунты в социальных сетях:");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1. YouTube: [https://www.youtube.com/channel/UCn9Zun7UjdOkJOGaArUgxJg]");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("2. Discord: [https://discordapp.com/users/987666543586467880/ or @ega_biba]");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("3. Roblox: [amogus1224_egor]");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("4. Donation Alerts: [https://donationalerts.com/r/amo1224gus]");
        Console.ResetColor();

        Console.WriteLine("===================================");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("5. И доработки кода, фич сделал [https://github.com/zzadryzz]");
        Console.ResetColor();

        Console.WriteLine(useEnglish ? "Press any key to exit..." : "Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static string GeneratePassword(int length, bool includeNumbers, bool includeUppercase, bool useKeyword, string keyword, string keywordLocation)
    {
        string chars = "abcdefghijklmnopqrstuvwxyz";
        if (includeNumbers)
        {
            chars += "0123456789";
        }
        if (includeUppercase)
        {
            chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }

        Random random = new Random();
        string password = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        if (useKeyword)
        {
            if (keywordLocation == "beginning")
            {
                password = keyword + password.Substring(keyword.Length);
            }
            else if (keywordLocation == "end")
            {
                password = password.Substring(0, password.Length - keyword.Length) + keyword;
            }
            else // keywordLocation == "anywhere"
            {
                int keywordPosition = random.Next(length - keyword.Length + 1);
                password = password.Substring(0, keywordPosition) + keyword + password.Substring(keywordPosition + keyword.Length);
            }
        }

        return password;
    }

    static void SaveLastPassword(string password)
    {
        try
        {
            File.WriteAllText("last_password.txt", password);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при сохранении последнего пароля: " + ex.Message);
        }
    }

    static string LoadLastPassword()
    {
        try
        {
            if (File.Exists("last_password.txt"))
            {
                return File.ReadAllText("last_password.txt");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при загрузке последнего пароля: " + ex.Message);
        }

        return "";
    }

    static bool AskToSavePassword(bool useEnglish)
    {
        Console.WriteLine(useEnglish ? "Хотите сохранить этот пароль? (да/нет):" : "Do you want to save this password? (yes/no):");
        string response = Console.ReadLine().Trim();
        return response.Equals(useEnglish ? "yes" : "да", StringComparison.OrdinalIgnoreCase);
    }

    static void CheckPasswordComplexity(string password, bool useEnglish)
    {
        int lengthScore = Math.Min(password.Length, 5);
        int digitScore = password.Any(char.IsDigit) ? 5 : 0;
        int uppercaseScore = password.Any(char.IsUpper) ? 5 : 0;
        int totalScore = lengthScore + digitScore + uppercaseScore;

        int maxScore = 15; // Максимальный балл

        Console.WriteLine(useEnglish ? $"Ваш пароль безопасен на {totalScore}/{maxScore}" : $"Your password is strong on {totalScore}/{maxScore}");
    }
}
