namespace InternViewServer.Models.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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

        //--------------------------------
        // This method Reads all Category
        //--------------------------------
        public List<Category> ReadCategory()
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

            cmd = CreateCommandWithStoredProcedure("SP_ReadAllCategory", con, null); // create the command

            List<Category> CategoryList = new List<Category>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // create the object that reads from SQL

                if (!dataReader.HasRows)
                {
                    return null;  // return null if doesn't found
                }

                while (dataReader.Read()) // brings record by record
                {
                    Category category = new Category();
                    category.Category_Id = Convert.ToInt32(dataReader["Category_Id"]);
                    category.CategoryName = dataReader["CategoryName"].ToString();
                    category.quantityAsFirst = (dataReader["quantityAsFirst"] != DBNull.Value) ? Convert.ToInt32(dataReader["quantityAsFirst"]) : 0;//בדיקה אם הערך במסד הנתונים הוא ריק תשים במקומו אפס
                    category.quantityAsSecond = (dataReader["quantityAsSecond"] != DBNull.Value) ? Convert.ToInt32(dataReader["quantityAsSecond"]) : 0;//בדיקה אם הערך במסד הנתונים הוא ריק תשים במקומו אפס

                    CategoryList.Add(category);
                }
                return CategoryList;
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
        // This method Reads all Intern_in_surgery
        //--------------------------------
        public List<Intern_in_surgery> ReadIntern_in_surgery()
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


            cmd = CreateCommandWithStoredProcedure("SP_ReadAllIntern_in_surgery", con, null);// create the command


            List<Intern_in_surgery> InternInSurgeryList = new List<Intern_in_surgery>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל


                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    Intern_in_surgery internInSurgery = new Intern_in_surgery();//צריך לבצע המרות כי חוזר אובייקט
                    //internInSurgery.id = Convert.ToInt32(dataReader["id"]);//המרות של טיפוסים 
                    internInSurgery.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);
                    internInSurgery.Intern_id = Convert.ToInt32(dataReader["Intern_id"]);
                    internInSurgery.Intern_role = dataReader["Intern_role"].ToString();
                    InternInSurgeryList.Add(internInSurgery);
                }
                return InternInSurgeryList;
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
        //-------------------------------------------
        // This method Reads all Procedure In Surgery
        //-------------------------------------------
        public List<ProcedureInSurgery> ReadProcedureInSurgery()
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


            cmd = CreateCommandWithStoredProcedure("SP_ReadProcedureInSurgery", con, null);// create the command


            List<ProcedureInSurgery> ProcedureInSurgeryList = new List<ProcedureInSurgery>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);// יצירת האובייקט שקורא מהסקיואל


                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    ProcedureInSurgery procedureInSurgery = new ProcedureInSurgery();//צריך לבצע המרות כי חוזר אובייקט
                    procedureInSurgery.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);
                    procedureInSurgery.procedure_Id = Convert.ToInt32(dataReader["procedure_Id"]);

                    ProcedureInSurgeryList.Add(procedureInSurgery);
                }
                return ProcedureInSurgeryList;
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
