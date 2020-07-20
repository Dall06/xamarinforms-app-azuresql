using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using api_db_azuresql.Position.Domain;
using api_db_azuresql.Responses;

namespace api_db_azuresql.Driver.Domain
{
    public class DriverModel
    {
        string ConnectionStr = "Server=tcp:db-azuresql-test-server.database.windows.net,1433;Initial Catalog=db-azuresql-test;Persist Security Info=False;User ID=azureSqlUser;Password=504203Dall;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Status { get; set; }
        public PositionModel ActualPosition { get; set; }

        public List<DriverModel> GetAll()
        {
            List<DriverModel> list = new List<DriverModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Driver INNER JOIN Position ON Driver.IdPosition = Position.Id;";
                    using SqlCommand cmd = new SqlCommand(tsql, conn);
                    using SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new DriverModel
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Status = reader["Status"].ToString(),
                            Picture = reader["Picture"].ToString(),
                            ActualPosition = new PositionModel
                            {
                                Id = (int)reader["IdPosition"],
                                Latitude = Convert.ToDouble(reader["Latitude"]),
                                Longitude = Convert.ToDouble(reader["Longitude"])
                            }
                        });
                    }
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ApiResponse Insert()
        {
            try
            {
                int r;
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    string tsql = "INSERT INTO Position (Longitude, Latitude)" +
                        "VALUES (@Longitude, @Latitude);" +
                        "INSERT INTO Driver (Name, Status, Picture, IdPosition)" +
                        "values (@Name, @Status, @Picture, SCOPE_IDENTITY());";
                    using SqlCommand cmd = new SqlCommand(tsql, conn)
                    {
                        CommandType = System.Data.CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Picture", Picture);
                    cmd.Parameters.AddWithValue("@Longitude", ActualPosition.Longitude);
                    cmd.Parameters.AddWithValue("@Latitude", ActualPosition.Latitude);
                    r = cmd.ExecuteNonQuery();
                }

                if (r > 0)
                {
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        Result = int.Parse(r.ToString()),
                        Message = "Driver user created"
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Result = 0,
                        Message = "Error during creation"
                    };
                }
            }
            catch (Exception exc)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = exc.Message
                };
            }
        }

        public ApiResponse Update()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    string tsql = "UPDATE Driver SET Name = @Name, Status = @Status, Picture = @Picture WHERE Id = @Id;" +
                        "UPDATE Position SET Longitude = @Longitude, Latitude = @Latitude FROM Driver, Position " +
                        "WHERE Driver.IdPosition = Position.Id AND Driver.Id = @Id";
                    using SqlCommand cmd = new SqlCommand(tsql, conn)
                    {
                        CommandType = System.Data.CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Picture", Picture);
                    cmd.Parameters.AddWithValue("@Longitude", ActualPosition.Longitude);
                    cmd.Parameters.AddWithValue("@Latitude", ActualPosition.Latitude);
                    cmd.ExecuteNonQuery();
                }
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = Id,
                    Message = "Updated"
                };
            }
            catch (Exception exc)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = exc.Message
                };
            }
        }

        public ApiResponse Delete(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionStr))
                {
                    conn.Open();
                    string tsql = "DECLARE @var1 int; SELECT @var1 = IdPosition FROM Driver WHERE Id = @Id;" +
                                   "DELETE FROM Driver WHERE Id = @Id; " +
                                   "DELETE FROM Position WHERE Id = @var1;";
                    using SqlCommand cmd = new SqlCommand(tsql, conn)
                    {
                        CommandType = System.Data.CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = id,
                    Message = "Deleted"
                };
            }
            catch (Exception exc)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = exc.Message
                };
            }
        }
    }
}
