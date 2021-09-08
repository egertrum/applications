using System;
using System.Collections.Generic;
using Domain.App;
using Domain.App.Identity;
using Domain.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.App.EF.AppDataInit
{
    public static class DataInit
    {
        public static void DropDatabase(AppDbContext context, ILogger logger)
        {
            logger.LogInformation("DropDatabase");
            context.Database.EnsureDeleted();
        }

        public static void MigrateDatabase(AppDbContext context, ILogger logger)
        {
            logger.LogInformation("MigrateDatabase");
            context.Database.Migrate();
        }
        
        public static void SeedData(AppDbContext context, ILogger logger)
        {
            SeedInitial(context, logger);
        }

        private static void SeedQuiz(string quizName, AppDbContext context, IEnumerable<(string, string, string, string)> data)
        {
            var counter = 1;
            var generalQuiz = new Quiz()
            {
                Name = quizName
            };
            // Answer 1 is always right!
            foreach (var (question, answer1, answer2, answer3) in data)
            {
                var questToDb = new Question()
                {
                    Value = question,
                    QuestionType = EQuestionType.Quiz
                };
                context.Question.Add(questToDb);
                context.SaveChanges();

                var quizQuestion = new QuizQuestion()
                {
                    Quiz = generalQuiz,
                    Question = questToDb,
                    Number = counter
                };
                context.QuizQuestion.Add(quizQuestion);
                context.SaveChanges();
                
                var questAnswer1 = new QuestionAnswer()
                {
                    Question = questToDb,
                    Value = answer1,
                    True = true
                };
                var questAnswer2 = new QuestionAnswer()
                {
                    Question = questToDb,
                    Value = answer2,
                    True = false
                };
                var questAnswer3 = new QuestionAnswer()
                {
                    Question = questToDb,
                    Value = answer3,
                    True = false
                };
                
                context.QuestionAnswer.Add(questAnswer1);
                context.QuestionAnswer.Add(questAnswer2);
                context.QuestionAnswer.Add(questAnswer3);
                context.SaveChanges();
                generalQuiz.MaxPoints++;
                counter++;
            }
        }
        
        private static void SeedPoll(string pollName, AppDbContext context, IEnumerable<(string, string, string, string)> data)
        {
            var counter = 1;
            var generalPoll = new Poll()
            {
                Name = pollName
            };
            // All answers are right!
            foreach (var (question, answer1, answer2, answer3) in data)
            {
                var questToDb = new Question()
                {
                    Value = question,
                    QuestionType = EQuestionType.Poll
                };
                context.Question.Add(questToDb);
                context.SaveChanges();

                var pollQuestion = new PollQuestion()
                {
                    Poll = generalPoll,
                    Question = questToDb,
                    Number = counter
                };
                context.PollQuestion.Add(pollQuestion);
                context.SaveChanges();
                
                var questAnswer1 = new QuestionAnswer()
                {
                    Question = questToDb,
                    Value = answer1,
                    True = true
                };
                var questAnswer2 = new QuestionAnswer()
                {
                    Question = questToDb,
                    Value = answer2,
                    True = true
                };
                var questAnswer3 = new QuestionAnswer()
                {
                    Question = questToDb,
                    Value = answer3,
                    True = true
                };
                
                context.QuestionAnswer.Add(questAnswer1);
                context.QuestionAnswer.Add(questAnswer2);
                context.QuestionAnswer.Add(questAnswer3);
                context.SaveChanges();

                counter++;
            }
        }

        private static void SeedInitial(AppDbContext context, ILogger logger)
        {
            SeedQuiz("General knowledge quiz", context, InitialData.GeneralQuestions);
            SeedQuiz("Maths quiz", context, InitialData.MathQuestions);
            SeedPoll("User experience", context, InitialData.UxPoll);
        }

        public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            ILogger logger)
        {
            logger.LogInformation("SeedIdentity");
            foreach (var (roleName, displayName) in InitialData.Roles)
            {
                var role = roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    role = new AppRole()
                    {
                        Name = roleName,
                        DisplayName = displayName
                    };

                    var result = roleManager.CreateAsync(role).Result;
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed");
                    }
                    logger.LogInformation("Seeded role {Role}", roleName);
                }
            }

            foreach (var userInfo in InitialData.Users)
            {
                var user = userManager.FindByNameAsync(userInfo.name).Result;
                if (user == null)
                {
                    user = new AppUser()
                    {
                        Email = userInfo.name,
                        UserName = userInfo.name,
                        FirstName = userInfo.firstName,
                        LastName = userInfo.lastName,
                        DOB = userInfo.DOB,
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(user, userInfo.password).Result;
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("User creation failed");
                    }
                    logger.LogInformation("Seeded user {User}", userInfo.name);

                }

                var roleResult = userManager.AddToRolesAsync(user, userInfo.roles).Result;
            }

        }


    }
}