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
        Console.WriteLine("      Password Generator");
        Console.WriteLine("===================================");

        string lastPassword = LoadLastPassword();

        if (!string.IsNullOrWhiteSpace(lastPassword))
        {
            Console.WriteLine("Last Generated Password / Последний сгенерированный пароль:");
            Console.WriteLine(lastPassword);
            Console.WriteLine();
        }

        Console.WriteLine("Choose a language / Выберите язык:");
        Console.WriteLine("1. English");
        Console.WriteLine("2. Русский");

        int languageChoice = int.Parse(Console.ReadLine());
        bool useEnglish = (languageChoice == 1);

        string languagePrompt = useEnglish ? "How many characters should the password have:" : "Сколько символов должно быть в пароле:";
        string includeNumbersPrompt = useEnglish ? "Include numbers in the password? (yes/no):" : "Будут ли цифры в пароле (да/нет):";
        string includeUppercasePrompt = useEnglish ? "Include uppercase letters in the password? (yes/no):" : "Будут ли заглавные буквы в пароле (да/нет):";

        Console.WriteLine(languagePrompt);
        int passwordLength = int.Parse(Console.ReadLine());

        Console.WriteLine(includeNumbersPrompt);
        bool includeNumbers = Console.ReadLine().ToLower() == (useEnglish ? "yes" : "да");

        Console.WriteLine(includeUppercasePrompt);
        bool includeUppercase = Console.ReadLine().ToLower() == (useEnglish ? "yes" : "да");

        Console.WriteLine(useEnglish ? "Include a keyword in the password? (Type the keyword or 'no' to skip):" : "Включить ключевое слово в пароль? (Введите ключевое слово или 'нет' для пропуска):");
        string keyword = Console.ReadLine();
        bool useKeyword = !string.Equals(keyword, (useEnglish ? "no" : "нет"), StringComparison.OrdinalIgnoreCase);

        string keywordLocation = "anywhere"; // По умолчанию размещаем ключевое слово где угодно
        if (useKeyword)
        {
            Console.WriteLine(useEnglish ? "Where would you like the keyword in the password? (1 for beginning, 2 for end, 3 for anywhere):" : "Где вы хотите разместить ключевое слово в пароле? (1 для начала, 2 для конца, 3 для произвольного места):");
            int keywordLocationChoice = int.Parse(Console.ReadLine());
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
        Console.WriteLine(useEnglish ? "Generated password: " + password : "Сгенерированный пароль: " + password);
        Console.WriteLine("===================================");

        SaveLastPassword(password);

        string thankYouMessage = useEnglish ? "Thank you for using the Password Generator!" : "Спасибо за использование генератора паролей!";
        Console.WriteLine(thankYouMessage);

        // Добавляем указание на YouTube, Discord, Roblox и Donation Alerts с цветовой разметкой
        Console.WriteLine("===================================");
        Console.WriteLine(useEnglish ? "Your Social Media Accounts:" : "Ваши аккаунты в социальных сетях:");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1. YouTube: [https://www.youtube.com/channel/UCn9Zun7UjdOkJOGaArUgxJg]");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("2. Discord: [https://discordapp.com/users/987666543586467880/ or @ega_biba]");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("3. Roblox: amogus1224_egor");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("4. Donation Alerts: donationalerts.com/r/amo1224gus");
        Console.ResetColor(); // Сбрасываем цвет фона и текста
        Console.WriteLine("===================================");

        // Просим пользователя нажать клавишу дважды перед закрытием
        Console.WriteLine(useEnglish ? "Press any key to exit..." : "Нажмите любую клавишу для выхода...");
        Console.ReadKey(); // Ожидаем два нажатия клавиши
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
                // Вставляем ключевое слово в случайное место в пароле
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
            Console.WriteLine("Error while saving the last password: " + ex.Message);
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
            Console.WriteLine("Error while loading the last password: " + ex.Message);
        }

        return "";
    }
}
