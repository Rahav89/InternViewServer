namespace InternViewServer.Models.DAL
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class DBservices
    {


        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        //--------------------------------
        // This method Reads all Interns
        //--------------------------------
        public List<Intern> ReadIntern()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            cmd = CreateCommandWithStoredProcedure("SP_ReadAllInterns", con, null);             // create the command


            List<Intern> InternList = new List<Intern>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל

                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    Intern intern = new Intern();//צריך לבצע המרות כי חוזר אובייקט
                    intern.Id = Convert.ToInt32(dataReader["Intern_id"]);//המרות של טיפוסים 
                    intern.Password_i = dataReader["Password_i"].ToString();
                    intern.First_name = dataReader["First_name"].ToString();
                    intern.Last_name = dataReader["Last_name"].ToString();
                    intern.Interns_year = dataReader["Interns_year"].ToString();
                    intern.Interns_rating = Convert.ToInt32(dataReader["Interns_rating"]);
                    intern.isManager = Convert.ToBoolean(dataReader["isManager"]);
                    intern.Email_I = dataReader["Email_I"].ToString();
                    InternList.Add(intern);
                }
                return InternList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //--------------------------------------------------------------------------------------------------
        // get intern by his ID
        //--------------------------------------------------------------------------------------------------
        public Intern GetInternByID(int internID)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internID);

            cmd = CreateCommandWithStoredProcedure("SP_getInternByID", con, paramDic); // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל

                if (dataReader.HasRows == false)
                {
                    return null;
                }

                Intern intern = new Intern();

                while (dataReader.Read())
                {
                    intern.Id = Convert.ToInt32(dataReader["Intern_id"]);//המרות של טיפוסים 
                    intern.Password_i = dataReader["Password_i"].ToString();
                    intern.First_name = dataReader["First_name"].ToString();
                    intern.Last_name = dataReader["Last_name"].ToString();
                    intern.Interns_year = dataReader["Interns_year"].ToString();
                    intern.Interns_rating = Convert.ToInt32(dataReader["Interns_rating"]);
                    intern.isManager = Convert.ToBoolean(dataReader["isManager"]);
                    intern.Email_I = dataReader["Email_I"].ToString();
                }

                return intern;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // check Email Intern
        //--------------------------------------------------------------------------------------------------
        public int checkEmailIntern(string email)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Email_intern", email);

            cmd = CreateCommandWithStoredProcedure("SP_checkEmailIntern", con, paramDic); // create the command
            var returnParameter = cmd.Parameters.Add("Exists", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            try
            {
                cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
            return (int)returnParameter.Value;
        }

        //--------------------------------------------------------------------------------------------------                                                              
        // Log in Intern                                                         
        //--------------------------------------------------------------------------------------------------
        public Intern LogInInternByIDPass(int id, string password)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", id);
            paramDic.Add("@Password_i", password);

            cmd = CreateCommandWithStoredProcedure("SP_LogIninternByIDPass", con, paramDic); // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל

                if (dataReader.HasRows == false)
                {
                    return null;
                }

                Intern intern = new Intern();

                while (dataReader.Read())
                {
                    intern.Id = Convert.ToInt32(dataReader["Intern_id"]);//המרות של טיפוסים 
                    intern.Password_i = dataReader["Password_i"].ToString();
                    intern.First_name = dataReader["First_name"].ToString();
                    intern.Last_name = dataReader["Last_name"].ToString();
                    intern.Interns_year = dataReader["Interns_year"].ToString();
                    intern.Interns_rating = Convert.ToInt32(dataReader["Interns_rating"]);
                    intern.isManager = Convert.ToBoolean(dataReader["isManager"]);
                    intern.Email_I = dataReader["Email_I"].ToString();
                }

                return intern;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //--------------------------------
        // This method Reads all  Surgeries
        //--------------------------------
        public List<Surgeries> ReadSurgeries()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            cmd = CreateCommandWithStoredProcedure("SP_ReadAllSurgeries", con, null);             // create the command


            List<Surgeries> SurgeriesList = new List<Surgeries>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל

                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    Surgeries surgery = new Surgeries();//צריך לבצע המרות כי חוזר אובייקט
                    surgery.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);//המרות של טיפוסים 
                    surgery.Case_number = Convert.ToInt32(dataReader["Case_number"]);
                    surgery.Patient_age = Convert.ToInt32(dataReader["Patient_age"]);
                    surgery.Surgery_date = Convert.ToDateTime(dataReader["Surgery_date"]);
                    surgery.Difficulty_level = Convert.ToInt32(dataReader["Difficulty_level"]);
                    surgery.Hospital_name = dataReader["Hospital_name"].ToString();
                    SurgeriesList.Add(surgery);
                }
                return SurgeriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method get surgery by id
        //--------------------------------------------------------------------------------------------------
        public List<Surgeries> GetSurgeriesByID(int surgeryID)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Surgery_id", surgeryID);

            cmd = CreateCommandWithStoredProcedure("SP_ReadSurgeryById", con, paramDic); // create the command


            List<Surgeries> SurgeriesList = new List<Surgeries>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל
                if (dataReader.HasRows == false)
                {
                    return null;  //return null if doesnt found
                }
                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    Surgeries surgery = new Surgeries();//צריך לבצע המרות כי חוזר אובייקט
                    surgery.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);//המרות של טיפוסים 
                    surgery.Case_number = Convert.ToInt32(dataReader["Case_number"]);
                    surgery.Patient_age = Convert.ToInt32(dataReader["Patient_age"]);
                    surgery.Surgery_date = Convert.ToDateTime(dataReader["Surgery_date"]);
                    surgery.Difficulty_level = Convert.ToInt32(dataReader["Difficulty_level"]);
                    SurgeriesList.Add(surgery);
                }
                return SurgeriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------
        // This method Reads all Procedure
        //--------------------------------
        public List<Procedure> ReadProcedure()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SP_ReadAllProcedure", con, null); // create the command

            List<Procedure> ProcedureList = new List<Procedure>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // create the object that reads from SQL

                if (!dataReader.HasRows)
                {
                    return null;  // return null if doesn't found
                }

                while (dataReader.Read()) // brings record by record
                {
                    Procedure procedure = new Procedure();
                    procedure.procedure_Id = Convert.ToInt32(dataReader["procedure_Id"]);
                    procedure.procedureName = dataReader["procedureName"].ToString();
                    procedure.category_Id = (dataReader["category_Id"] != DBNull.Value) ? Convert.ToInt32(dataReader["category_Id"]) : 0;
                    procedure.quantityAsMain = (dataReader["quantityAsMain"] != DBNull.Value) ? Convert.ToInt32(dataReader["quantityAsMain"]) : 0;//בדיקה אם הערך במסד הנתונים הוא ריק תשים במקומו אפס
                    procedure.quantityAsFirst = (dataReader["quantityAsFirst"] != DBNull.Value) ? Convert.ToInt32(dataReader["quantityAsFirst"]) : 0;//בדיקה אם הערך במסד הנתונים הוא ריק תשים במקומו אפס
                    procedure.quantityAsSecond = (dataReader["quantityAsSecond"] != DBNull.Value) ? Convert.ToInt32(dataReader["quantityAsSecond"]) : 0;//בדיקה אם הערך במסד הנתונים הוא ריק תשים במקומו אפס

                    ProcedureList.Add(procedure);
                }
                return ProcedureList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------
        // This method get All teh surgeries done by the intern
        //--------------------------------------------------------------------------------------------------
        public List<SurgeriesOfIntern> AllInternSurgeries(int internId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);


            cmd = CreateCommandWithStoredProcedure("SP_SurgeriesByInternID", con, paramDic); // create the command


            List<SurgeriesOfIntern> FiveRecentInternSurgeriesList = new List<SurgeriesOfIntern>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל

                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    SurgeriesOfIntern recentSurgeriesOfIntern = new SurgeriesOfIntern();//צריך לבצע המרות כי חוזר אובייקט
                    recentSurgeriesOfIntern.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);//המרות של טיפוסים 
                    recentSurgeriesOfIntern.procedureName = dataReader["procedureName"].ToString();
                    recentSurgeriesOfIntern.Intern_role = dataReader["Intern_role"].ToString();
                    recentSurgeriesOfIntern.Case_number = Convert.ToInt32(dataReader["Case_number"]);
                    recentSurgeriesOfIntern.Patient_age = Convert.ToInt32(dataReader["Patient_age"]);
                    recentSurgeriesOfIntern.Surgery_date = Convert.ToDateTime(dataReader["Surgery_date"]);
                    recentSurgeriesOfIntern.Difficulty_level = Convert.ToInt32(dataReader["Difficulty_level"]);
                    FiveRecentInternSurgeriesList.Add(recentSurgeriesOfIntern);
                }
                return FiveRecentInternSurgeriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method get 5 recent surgeries done by the intern, order by date
        //--------------------------------------------------------------------------------------------------

        public List<SurgeriesOfIntern> FiveRecentInternSurgeries(int internId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);


            cmd = CreateCommandWithStoredProcedure("SP_FiveRecentInternSurgeries", con, paramDic); // create the command


            List<SurgeriesOfIntern> FiveRecentInternSurgeriesList = new List<SurgeriesOfIntern>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל

                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    SurgeriesOfIntern recentSurgeriesOfIntern = new SurgeriesOfIntern();//צריך לבצע המרות כי חוזר אובייקט
                    recentSurgeriesOfIntern.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);//המרות של טיפוסים 
                    recentSurgeriesOfIntern.procedureName = dataReader["procedureName"].ToString();
                    recentSurgeriesOfIntern.Intern_role = dataReader["Intern_role"].ToString();
                    recentSurgeriesOfIntern.Case_number = Convert.ToInt32(dataReader["Case_number"]);
                    recentSurgeriesOfIntern.Patient_age = Convert.ToInt32(dataReader["Patient_age"]);
                    recentSurgeriesOfIntern.Surgery_date = Convert.ToDateTime(dataReader["Surgery_date"]);
                    recentSurgeriesOfIntern.Difficulty_level = Convert.ToInt32(dataReader["Difficulty_level"]);
                    FiveRecentInternSurgeriesList.Add(recentSurgeriesOfIntern);
                }
                return FiveRecentInternSurgeriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //--------------------------------------------------------------------------------------------------
        // This method the Syllabus of the intern
        //--------------------------------------------------------------------------------------------------
        public List<SyllabusOfIntern> GetSyllabusOfIntern(int internId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);


            cmd = CreateCommandWithStoredProcedure("SP_SyllabusOfIntern", con, paramDic); // create the command


            List<SyllabusOfIntern> syllabusOfIntern = new List<SyllabusOfIntern>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל

                while (dataReader.Read())//מביאה רשומה רשומה 
                {

                    SyllabusOfIntern rowOfSyllabusOfIntern = new SyllabusOfIntern();//צריך לבצע המרות כי חוזר אובייקט
                    rowOfSyllabusOfIntern.procedure_Id = Convert.ToInt32(dataReader["procedure_Id"]);
                    rowOfSyllabusOfIntern.procedureName = dataReader["procedureName"].ToString();
                    rowOfSyllabusOfIntern.syllabus = Convert.ToInt32(dataReader["syllabus"]);
                    rowOfSyllabusOfIntern.haveDone = Convert.ToInt32(dataReader["haveDone"]);
                    rowOfSyllabusOfIntern.need = Convert.ToInt32(dataReader["need"]);
                    syllabusOfIntern.Add(rowOfSyllabusOfIntern);
                }
                return syllabusOfIntern;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        ////--------------------------------------------------------------------------------------------------
        ////This method update a user to the user table 
        ////--------------------------------------------------------------------------------------------------
        public int UpdateIntern(Intern intern)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@Intern_id", intern.Id);
                paramDic.Add("@Password_i", intern.Password_i);
                paramDic.Add("@First_name", intern.First_name);
                paramDic.Add("@Last_name", intern.Last_name);
                paramDic.Add("@Interns_year", intern.Interns_year);
                paramDic.Add("@Interns_rating", intern.Interns_rating);
                paramDic.Add("@isManager", intern.isManager);
                paramDic.Add("@Email_I", intern.Email_I);

                cmd = CreateCommandWithStoredProcedure("SP_UpdateUser", con, paramDic); // create the command
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected; // return the number of records affected
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // Rethrow the exception after logging it
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed in the finally block
                }
            }
        }

        //----------------------------
        //sets new password for intern by email
        //----------------------------
        public int UpdateInternPassword(string email, string password)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@email", email);
                paramDic.Add("@newPass", password);

                cmd = CreateCommandWithStoredProcedure("SP_newPassIntern", con, paramDic); // create the command
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected; // return the number of records affected
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // Rethrow the exception after logging it
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed in the finally block
                }
            }
        }
        //----------------------------
        //gets all the interns and their procedure count
        //----------------------------
        public List<InternProcedureCounter> InternProcedureSummary()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();


            cmd = CreateCommandWithStoredProcedure("SP_CountProceduresByIntern", con, paramDic); // create the command

            List<InternProcedureCounter> summaries = new List<InternProcedureCounter>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    InternProcedureCounter summary = new InternProcedureCounter();
                    summary.InternId = Convert.ToInt32(dataReader["Intern_id"]);
                    summary.FirstName = Convert.ToString(dataReader["First_name"]);
                    summary.LastName = Convert.ToString(dataReader["Last_name"]);
                    summary.InternsRating = Convert.ToInt32(dataReader["Interns_rating"]);
                    summary.InternsYear = Convert.ToString(dataReader["Interns_year"]);
                    summary.ProcedureCount = Convert.ToInt32(dataReader["ProcedureCount"]);
                    summary.OverallNeed = Convert.ToInt32(dataReader["overAllNeed"]);
                    summaries.Add(summary);
                }
                return summaries;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        //----------------------------
        //gets all intern's procedure count by main/first/secound
        //----------------------------
        public List<DetailedSyllabusOfIntern> fullDetailedSyllabusOfIntern(int internId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@Intern_id", internId);
            cmd = CreateCommandWithStoredProcedure("SP_GetInternDetailedSyllabus", con, paramDic); // create the command

            List<DetailedSyllabusOfIntern> summaries = new List<DetailedSyllabusOfIntern>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    DetailedSyllabusOfIntern summary = new DetailedSyllabusOfIntern();
                    summary.procedureName = Convert.ToString(dataReader["procedureName"]);
                    summary.category_Id = Convert.ToInt32(dataReader["category_Id"]);
                    summary.CategoryName = Convert.ToString(dataReader["CategoryName"]);
                    summary.requiredAsMain = Convert.ToInt32(dataReader["requiredAsMain"]);
                    summary.requiredAsFirst = Convert.ToInt32(dataReader["requiredAsFirst"]);
                    summary.requiredAsSecond = Convert.ToInt32(dataReader["requiredAsSecond"]);
                    summary.doneAsMain = Convert.ToInt32(dataReader["doneAsMain"]);
                    summary.doneAsFirst = Convert.ToInt32(dataReader["doneAsFirst"]);
                    summary.doneAsSecond = Convert.ToInt32(dataReader["doneAsSecond"]);
                    summary.categoryRequiredFirst = Convert.ToInt32(dataReader["categoryRequiredFirst"]);
                    summary.categoryRequiredSecond = Convert.ToInt32(dataReader["categoryRequiredSecond"]);
                    summaries.Add(summary);
                }
                return summaries;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //-----------------------------------
        //Get Intern Surgeries By Procedure
        //----------------------------------
        public List<Dictionary<string, object>> GetInternSurgeriesByProcedure(int internId, int procedureID)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@InternID", internId);
            paramDic.Add("@ProcedureID", procedureID);

            cmd = CreateCommandWithStoredProcedure("SP_GetInternSurgeriesByProcedure", con, paramDic); // create the command

            List<Dictionary<string, object>> internSBPList = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> surgeryDetails = new Dictionary<string, object>
                    {
                        {"Surgery_date", Convert.ToDateTime(dataReader["Surgery_date"])},
                        {"Difficulty_level", Convert.ToInt32(dataReader["Difficulty_level"])},
                        {"Hospital_name",Convert.ToString(dataReader["Hospital_name"])},
                        {"Procedure_name", Convert.ToString(dataReader["procedureName"])},
                        {"Intern_role", Convert.ToString(dataReader["Intern_role"])}
                    };

                    internSBPList.Add(surgeryDetails);
                }
                return internSBPList;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //-----------------------------------
        //Get all the interns exept from the one given - the ones he can talk to
        //----------------------------------
        public List<Dictionary<string, object>> GetInternsForChat(int internId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);

            cmd = CreateCommandWithStoredProcedure("SP_GetInternsForChat", con, paramDic); // create the command

            List<Dictionary<string, object>> interns = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> intern = new Dictionary<string, object>
                    {
                        {"Intern_id", Convert.ToInt32(dataReader["Intern_id"])},
                        {"First_name",Convert.ToString(dataReader["First_name"])},
                        {"Last_name", Convert.ToString(dataReader["Last_name"])},
                    };

                    interns.Add(intern);
                }
                return interns;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //-----------------------------------
        // Get the last messages the intern has to each one of the other interns
        //----------------------------------
        public List<Dictionary<string, object>> GetLastMessagesForIntern(int internId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);

            cmd = CreateCommandWithStoredProcedure("SP_GetLastMessagesForIntern", con, paramDic); // create the command

            List<Dictionary<string, object>> messages = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> message = new Dictionary<string, object>
                    {
                        {"messages_id", Convert.ToInt32(dataReader["messages_id"])},
                        {"from_id", Convert.ToInt32(dataReader["from_id"])},
                        {"to_id", Convert.ToInt32(dataReader["to_id"])},
                        {"content",Convert.ToString(dataReader["content"])},
                        {"partner_id", Convert.ToString(dataReader["partner_id"])},
                    };

                    messages.Add(message);
                }
                return messages;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //-----------------------------------
        // Get all the messages the intern has with the other intern
        //----------------------------------
        public List<Message> GetChatWithPartner(int internId, int intern_Partner_id)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);
            paramDic.Add("@Intern_Partner_id", intern_Partner_id);

            cmd = CreateCommandWithStoredProcedure("SP_GetChatWithPartner", con, paramDic); // create the command

            List<Message> messages = new List<Message>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Message message = new Message();
                    message.messages_id = Convert.ToInt32(dataReader["messages_id"]);
                    message.from_id = Convert.ToInt32(dataReader["from_id"]);
                    message.to_id = Convert.ToInt32(dataReader["to_id"]);
                    message.content = Convert.ToString(dataReader["content"]);
                    message.messages_date = Convert.ToDateTime(dataReader["messages_date"]);
                    messages.Add(message);

                }
                return messages;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //-------------------------------------------
        /// insert new Message
        //------------------------------------------
        public int AddNewMessage(Message m)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@from_id", m.from_id);
            paramDic.Add("@to_id", m.to_id);
            paramDic.Add("@content", m.content);
            paramDic.Add("@messages_date", m.messages_date);

            cmd = CreateCommandWithStoredProcedure("SP_InsertNewMessage", con, paramDic);   // create the command
            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command              
                return numEffected; // return the number of records affected
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }



        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }
    }
}






