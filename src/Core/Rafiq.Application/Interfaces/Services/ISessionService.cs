//namespace Rafiq.Application.Interfaces.Services;

//public interface ISessionService
//{
//    //  Specialist
//    #region Specialist 
//    Task<int> CreateSessionAsync(SessionCreateDto dto);
//    Task<IEnumerable<SessionDto>> GetSessionsBySpecialistAsync(string specialistId);
//    #endregion

//    #region Patient
//    Task<IEnumerable<SessionDto>> GetSessionsForPatientAsync(int patientId);
//    Task<SessionDto?> GetLastSessionForPatientAsync(int patientId);
//    Task<SessionDto?> GetSessionDetailsAsync(int sessionId, int patientId);
//    #endregion

//    // Video URL for patient to access the session recording. 
//    // The sessionId is the unique identifier for the session.
//    // The patientId is used to verify that the patient has access to the session.
//    Task<string> GetSignedVideoUrlAsync(int sessionId, int patientId);
//}
