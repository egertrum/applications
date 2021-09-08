using System;

namespace DAL.App.EF.AppDataInit
{
    public static class InitialData
    {
        public static readonly (string roleName, string displayName)[] Roles =
        {
            ("Admin", "Administrator"),
            ("User", "User"),
        };

        public static readonly (string name, string password, string firstName, string lastName, DateTime DOB, string[] roles)[] Users =
        {
            ("admin@egrumj.com", "Foo.bar1", "Admin", "egrumj", DateTime.Parse("2000-01-01"), new []{"Admin"}),
            ("user@egrumj.ee", "Foo.bar1", "User", "egrumj", DateTime.Parse("2000-01-01"), new string[0]),
        };
        
        public static readonly (string question, string answer1, string answer2, string answer3)[] GeneralQuestions =
        {
            ("What is 9 + 10?", "19", "21", "18"),
            ("How old is Donald Trump?", "74", "69", "55"),
            ("From which language is the word ‘ketchup’ derived?", "Chinese", "Japanese", "Estonian"),
            ("Which is the country with the biggest population in Europe?", "Russia", "France", "Germany"),
            ("What colour are the four stars on the flag of New Zealand?", "Red", "Yellow", "Blue"),
        };
        
        public static readonly (string question, string answer1, string answer2, string answer3)[] MathQuestions =
        {
            ("9 x 10", "90", "900", "9000"),
            ("7 x 8", "56", "54", "62"),
            ("7 x 16", "112", "92", "102"),
            ("3 x 31", "93", "76", "96"),
            ("108 / 4", "27", "32", "14"),
            ("27 / 9", "3", "4", "6"),
            ("252 / 2", "126", "128", "124"),
            ("69 / 3", "23", "33", "13")
        };
        
        public static readonly (string question, string answer1, string answer2, string answer3)[] UxPoll =
        {
            ("Do you like the design?", "Yes", "No", "Kind of"),
            ("Are you satisfied with the experience on the page?", "Yes", "No", "Kind of"),
            ("Do you like the font used on this app?", "Yes", "No", "Kind of"),
            ("Do you feel like there is something missing from the page?", "Yes", "No", "Kind of"),
            ("Would you like to see more statistics?", "Yes", "No", "Kind of"),
            ("Is the answering of quizzes structured well?", "Yes", "No", "Kind of"),
        };

    }
    
}