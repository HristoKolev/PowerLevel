// ReSharper disable InconsistentNaming
// ReSharper disable HeuristicUnreachableCode

namespace PowerLevel.Server;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Mapping;
using Npgsql;
using NpgsqlTypes;
using Xdxd.DotNet.Postgres;

/// <summary>
/// <para>Table name: 'quiz_answers'.</para>
/// <para>Table schema: 'public'.</para>
/// </summary>
[Table(Schema = "public", Name = "quiz_answers")]
[ExcludeFromCodeCoverage]
public class QuizAnswerPoco : IPoco<QuizAnswerPoco>
{
    /// <summary>
    /// <para>Column name: 'answer_content'.</para>
    /// <para>Table name: 'quiz_answers'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "answer_content", DataType = DataType.Text)]
    public string AnswerContent { get; set; }

    /// <summary>
    /// <para>Column name: 'answer_id'.</para>
    /// <para>Table name: 'quiz_answers'.</para>
    /// <para>Primary key of table: 'quiz_answers'.</para>
    /// <para>Primary key constraint name: 'quiz_answers_pkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [PrimaryKey]
    [Identity]
    [Column(Name = "answer_id", DataType = DataType.Int32)]
    public int AnswerID { get; set; }

    /// <summary>
    /// <para>Column name: 'answer_is_correct'.</para>
    /// <para>Table name: 'quiz_answers'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'boolean'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Boolean'.</para>
    /// <para>CLR type: 'bool'.</para>
    /// <para>linq2db data type: 'DataType.Boolean'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "answer_is_correct", DataType = DataType.Boolean)]
    public bool AnswerIsCorrect { get; set; }

    /// <summary>
    /// <para>Column name: 'answer_position'.</para>
    /// <para>Table name: 'quiz_answers'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "answer_position", DataType = DataType.Int32)]
    public int AnswerPosition { get; set; }

    /// <summary>
    /// <para>Column name: 'question_id'.</para>
    /// <para>Table name: 'quiz_answers'.</para>
    /// <para>Foreign key column [public.quiz_answers.question_id -> public.quiz_questions.question_id].</para>
    /// <para>Foreign key constraint name: 'quiz_answers_question_id_fkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "question_id", DataType = DataType.Int32)]
    public int QuestionID { get; set; }

    public NpgsqlParameter[] GetNonPkParameters()
    {
        // ReSharper disable once RedundantExplicitArrayCreation
        return new NpgsqlParameter[]
        {
            new NpgsqlParameter<string>
            {
                TypedValue = this.AnswerContent,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<bool>
            {
                TypedValue = this.AnswerIsCorrect,
                NpgsqlDbType = NpgsqlDbType.Boolean,
            },
            new NpgsqlParameter<int>
            {
                TypedValue = this.AnswerPosition,
                NpgsqlDbType = NpgsqlDbType.Integer,
            },
            new NpgsqlParameter<int>
            {
                TypedValue = this.QuestionID,
                NpgsqlDbType = NpgsqlDbType.Integer,
            },
        };
    }

    public int GetPrimaryKey()
    {
        return this.AnswerID;
    }

    public void SetPrimaryKey(int value)
    {
        this.AnswerID = value;
    }

    public bool IsNew()
    {
        return this.AnswerID == default;
    }

    public async Task WriteToImporter(NpgsqlBinaryImporter importer)
    {
        if (this.AnswerContent == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.AnswerContent, NpgsqlDbType.Text);
        }

        await importer.WriteAsync(this.AnswerIsCorrect, NpgsqlDbType.Boolean);

        await importer.WriteAsync(this.AnswerPosition, NpgsqlDbType.Integer);

        await importer.WriteAsync(this.QuestionID, NpgsqlDbType.Integer);
    }

    public static TableMetadataModel Metadata => DbMetadata.QuizAnswerPocoMetadata;
}

/// <summary>
/// <para>Table name: 'quiz_questions'.</para>
/// <para>Table schema: 'public'.</para>
/// </summary>
[Table(Schema = "public", Name = "quiz_questions")]
[ExcludeFromCodeCoverage]
public class QuizQuestionPoco : IPoco<QuizQuestionPoco>
{
    /// <summary>
    /// <para>Column name: 'question_content'.</para>
    /// <para>Table name: 'quiz_questions'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "question_content", DataType = DataType.Text)]
    public string QuestionContent { get; set; }

    /// <summary>
    /// <para>Column name: 'question_id'.</para>
    /// <para>Table name: 'quiz_questions'.</para>
    /// <para>Primary key of table: 'quiz_questions'.</para>
    /// <para>Primary key constraint name: 'quiz_questions_pkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [PrimaryKey]
    [Identity]
    [Column(Name = "question_id", DataType = DataType.Int32)]
    public int QuestionID { get; set; }

    /// <summary>
    /// <para>Column name: 'question_name'.</para>
    /// <para>Table name: 'quiz_questions'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "question_name", DataType = DataType.Text)]
    public string QuestionName { get; set; }

    /// <summary>
    /// <para>Column name: 'question_position'.</para>
    /// <para>Table name: 'quiz_questions'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "question_position", DataType = DataType.Int32)]
    public int QuestionPosition { get; set; }

    /// <summary>
    /// <para>Column name: 'quiz_id'.</para>
    /// <para>Table name: 'quiz_questions'.</para>
    /// <para>Foreign key column [public.quiz_questions.quiz_id -> public.quizzes.quiz_id].</para>
    /// <para>Foreign key constraint name: 'quiz_questions_quiz_id_fkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "quiz_id", DataType = DataType.Int32)]
    public int QuizID { get; set; }

    public NpgsqlParameter[] GetNonPkParameters()
    {
        // ReSharper disable once RedundantExplicitArrayCreation
        return new NpgsqlParameter[]
        {
            new NpgsqlParameter<string>
            {
                TypedValue = this.QuestionContent,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<string>
            {
                TypedValue = this.QuestionName,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<int>
            {
                TypedValue = this.QuestionPosition,
                NpgsqlDbType = NpgsqlDbType.Integer,
            },
            new NpgsqlParameter<int>
            {
                TypedValue = this.QuizID,
                NpgsqlDbType = NpgsqlDbType.Integer,
            },
        };
    }

    public int GetPrimaryKey()
    {
        return this.QuestionID;
    }

    public void SetPrimaryKey(int value)
    {
        this.QuestionID = value;
    }

    public bool IsNew()
    {
        return this.QuestionID == default;
    }

    public async Task WriteToImporter(NpgsqlBinaryImporter importer)
    {
        if (this.QuestionContent == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.QuestionContent, NpgsqlDbType.Text);
        }

        if (this.QuestionName == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.QuestionName, NpgsqlDbType.Text);
        }

        await importer.WriteAsync(this.QuestionPosition, NpgsqlDbType.Integer);

        await importer.WriteAsync(this.QuizID, NpgsqlDbType.Integer);
    }

    public static TableMetadataModel Metadata => DbMetadata.QuizQuestionPocoMetadata;
}

/// <summary>
/// <para>Table name: 'quizzes'.</para>
/// <para>Table schema: 'public'.</para>
/// </summary>
[Table(Schema = "public", Name = "quizzes")]
[ExcludeFromCodeCoverage]
public class QuizPoco : IPoco<QuizPoco>
{
    /// <summary>
    /// <para>Column name: 'quiz_id'.</para>
    /// <para>Table name: 'quizzes'.</para>
    /// <para>Primary key of table: 'quizzes'.</para>
    /// <para>Primary key constraint name: 'quizzes_pkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [PrimaryKey]
    [Identity]
    [Column(Name = "quiz_id", DataType = DataType.Int32)]
    public int QuizID { get; set; }

    /// <summary>
    /// <para>Column name: 'quiz_name'.</para>
    /// <para>Table name: 'quizzes'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "quiz_name", DataType = DataType.Text)]
    public string QuizName { get; set; }

    /// <summary>
    /// <para>Column name: 'user_profile_id'.</para>
    /// <para>Table name: 'quizzes'.</para>
    /// <para>Foreign key column [public.quizzes.user_profile_id -> public.user_profiles.user_profile_id].</para>
    /// <para>Foreign key constraint name: 'quizzes_user_profile_id_fkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "user_profile_id", DataType = DataType.Int32)]
    public int UserProfileID { get; set; }

    public NpgsqlParameter[] GetNonPkParameters()
    {
        // ReSharper disable once RedundantExplicitArrayCreation
        return new NpgsqlParameter[]
        {
            new NpgsqlParameter<string>
            {
                TypedValue = this.QuizName,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<int>
            {
                TypedValue = this.UserProfileID,
                NpgsqlDbType = NpgsqlDbType.Integer,
            },
        };
    }

    public int GetPrimaryKey()
    {
        return this.QuizID;
    }

    public void SetPrimaryKey(int value)
    {
        this.QuizID = value;
    }

    public bool IsNew()
    {
        return this.QuizID == default;
    }

    public async Task WriteToImporter(NpgsqlBinaryImporter importer)
    {
        if (this.QuizName == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.QuizName, NpgsqlDbType.Text);
        }

        await importer.WriteAsync(this.UserProfileID, NpgsqlDbType.Integer);
    }

    public static TableMetadataModel Metadata => DbMetadata.QuizPocoMetadata;
}

/// <summary>
/// <para>Table name: 'user_logins'.</para>
/// <para>Table schema: 'public'.</para>
/// </summary>
[Table(Schema = "public", Name = "user_logins")]
[ExcludeFromCodeCoverage]
public class UserLoginPoco : IPoco<UserLoginPoco>
{
    /// <summary>
    /// <para>Column name: 'email_address'.</para>
    /// <para>Table name: 'user_logins'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "email_address", DataType = DataType.Text)]
    public string EmailAddress { get; set; }

    /// <summary>
    /// <para>Column name: 'enabled'.</para>
    /// <para>Table name: 'user_logins'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'boolean'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Boolean'.</para>
    /// <para>CLR type: 'bool'.</para>
    /// <para>linq2db data type: 'DataType.Boolean'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "enabled", DataType = DataType.Boolean)]
    public bool Enabled { get; set; }

    /// <summary>
    /// <para>Column name: 'login_id'.</para>
    /// <para>Table name: 'user_logins'.</para>
    /// <para>Primary key of table: 'user_logins'.</para>
    /// <para>Primary key constraint name: 'user_logins_pkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [PrimaryKey]
    [Identity]
    [Column(Name = "login_id", DataType = DataType.Int32)]
    public int LoginID { get; set; }

    /// <summary>
    /// <para>Column name: 'password_hash'.</para>
    /// <para>Table name: 'user_logins'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "password_hash", DataType = DataType.Text)]
    public string PasswordHash { get; set; }

    /// <summary>
    /// <para>Column name: 'user_profile_id'.</para>
    /// <para>Table name: 'user_logins'.</para>
    /// <para>Foreign key column [public.user_logins.user_profile_id -> public.user_profiles.user_profile_id].</para>
    /// <para>Foreign key constraint name: 'user_logins_user_profile_id_fkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "user_profile_id", DataType = DataType.Int32)]
    public int UserProfileID { get; set; }

    /// <summary>
    /// <para>Column name: 'verification_code'.</para>
    /// <para>Table name: 'user_logins'.</para>
    /// <para>This column is nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [Nullable]
    [Column(Name = "verification_code", DataType = DataType.Text)]
    public string VerificationCode { get; set; }

    /// <summary>
    /// <para>Column name: 'verified'.</para>
    /// <para>Table name: 'user_logins'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'boolean'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Boolean'.</para>
    /// <para>CLR type: 'bool'.</para>
    /// <para>linq2db data type: 'DataType.Boolean'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "verified", DataType = DataType.Boolean)]
    public bool Verified { get; set; }

    public NpgsqlParameter[] GetNonPkParameters()
    {
        // ReSharper disable once RedundantExplicitArrayCreation
        return new NpgsqlParameter[]
        {
            new NpgsqlParameter<string>
            {
                TypedValue = this.EmailAddress,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<bool>
            {
                TypedValue = this.Enabled,
                NpgsqlDbType = NpgsqlDbType.Boolean,
            },
            new NpgsqlParameter<string>
            {
                TypedValue = this.PasswordHash,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<int>
            {
                TypedValue = this.UserProfileID,
                NpgsqlDbType = NpgsqlDbType.Integer,
            },
            new NpgsqlParameter<string>
            {
                TypedValue = this.VerificationCode,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<bool>
            {
                TypedValue = this.Verified,
                NpgsqlDbType = NpgsqlDbType.Boolean,
            },
        };
    }

    public int GetPrimaryKey()
    {
        return this.LoginID;
    }

    public void SetPrimaryKey(int value)
    {
        this.LoginID = value;
    }

    public bool IsNew()
    {
        return this.LoginID == default;
    }

    public async Task WriteToImporter(NpgsqlBinaryImporter importer)
    {
        if (this.EmailAddress == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.EmailAddress, NpgsqlDbType.Text);
        }

        await importer.WriteAsync(this.Enabled, NpgsqlDbType.Boolean);

        if (this.PasswordHash == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.PasswordHash, NpgsqlDbType.Text);
        }

        await importer.WriteAsync(this.UserProfileID, NpgsqlDbType.Integer);

        if (this.VerificationCode == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.VerificationCode, NpgsqlDbType.Text);
        }

        await importer.WriteAsync(this.Verified, NpgsqlDbType.Boolean);
    }

    public static TableMetadataModel Metadata => DbMetadata.UserLoginPocoMetadata;
}

/// <summary>
/// <para>Table name: 'user_profiles'.</para>
/// <para>Table schema: 'public'.</para>
/// </summary>
[Table(Schema = "public", Name = "user_profiles")]
[ExcludeFromCodeCoverage]
public class UserProfilePoco : IPoco<UserProfilePoco>
{
    /// <summary>
    /// <para>Column name: 'email_address'.</para>
    /// <para>Table name: 'user_profiles'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "email_address", DataType = DataType.Text)]
    public string EmailAddress { get; set; }

    /// <summary>
    /// <para>Column name: 'registration_date'.</para>
    /// <para>Table name: 'user_profiles'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'timestamp with time zone'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.TimestampTz'.</para>
    /// <para>CLR type: 'DateTime'.</para>
    /// <para>linq2db data type: 'DataType.DateTime2'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "registration_date", DataType = DataType.DateTime2)]
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// <para>Column name: 'user_profile_id'.</para>
    /// <para>Table name: 'user_profiles'.</para>
    /// <para>Primary key of table: 'user_profiles'.</para>
    /// <para>Primary key constraint name: 'user_profiles_pkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [PrimaryKey]
    [Identity]
    [Column(Name = "user_profile_id", DataType = DataType.Int32)]
    public int UserProfileID { get; set; }

    public NpgsqlParameter[] GetNonPkParameters()
    {
        // ReSharper disable once RedundantExplicitArrayCreation
        return new NpgsqlParameter[]
        {
            new NpgsqlParameter<string>
            {
                TypedValue = this.EmailAddress,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<DateTime>
            {
                TypedValue = this.RegistrationDate,
                NpgsqlDbType = NpgsqlDbType.TimestampTz,
            },
        };
    }

    public int GetPrimaryKey()
    {
        return this.UserProfileID;
    }

    public void SetPrimaryKey(int value)
    {
        this.UserProfileID = value;
    }

    public bool IsNew()
    {
        return this.UserProfileID == default;
    }

    public async Task WriteToImporter(NpgsqlBinaryImporter importer)
    {
        if (this.EmailAddress == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.EmailAddress, NpgsqlDbType.Text);
        }

        await importer.WriteAsync(this.RegistrationDate, NpgsqlDbType.TimestampTz);
    }

    public static TableMetadataModel Metadata => DbMetadata.UserProfilePocoMetadata;
}

/// <summary>
/// <para>Table name: 'user_sessions'.</para>
/// <para>Table schema: 'public'.</para>
/// </summary>
[Table(Schema = "public", Name = "user_sessions")]
[ExcludeFromCodeCoverage]
public class UserSessionPoco : IPoco<UserSessionPoco>
{
    /// <summary>
    /// <para>Column name: 'csrf_token_hash'.</para>
    /// <para>Table name: 'user_sessions'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'text'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Text'.</para>
    /// <para>CLR type: 'string'.</para>
    /// <para>linq2db data type: 'DataType.Text'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "csrf_token_hash", DataType = DataType.Text)]
    public string CsrfTokenHash { get; set; }

    /// <summary>
    /// <para>Column name: 'expiration_date'.</para>
    /// <para>Table name: 'user_sessions'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'timestamp with time zone'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.TimestampTz'.</para>
    /// <para>CLR type: 'DateTime'.</para>
    /// <para>linq2db data type: 'DataType.DateTime2'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "expiration_date", DataType = DataType.DateTime2)]
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// <para>Column name: 'logged_out'.</para>
    /// <para>Table name: 'user_sessions'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'boolean'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Boolean'.</para>
    /// <para>CLR type: 'bool'.</para>
    /// <para>linq2db data type: 'DataType.Boolean'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "logged_out", DataType = DataType.Boolean)]
    public bool LoggedOut { get; set; }

    /// <summary>
    /// <para>Column name: 'login_date'.</para>
    /// <para>Table name: 'user_sessions'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'timestamp with time zone'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.TimestampTz'.</para>
    /// <para>CLR type: 'DateTime'.</para>
    /// <para>linq2db data type: 'DataType.DateTime2'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "login_date", DataType = DataType.DateTime2)]
    public DateTime LoginDate { get; set; }

    /// <summary>
    /// <para>Column name: 'login_id'.</para>
    /// <para>Table name: 'user_sessions'.</para>
    /// <para>Foreign key column [public.user_sessions.login_id -> public.user_logins.login_id].</para>
    /// <para>Foreign key constraint name: 'user_sessions_login_id_fkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "login_id", DataType = DataType.Int32)]
    public int LoginID { get; set; }

    /// <summary>
    /// <para>Column name: 'logout_date'.</para>
    /// <para>Table name: 'user_sessions'.</para>
    /// <para>This column is nullable.</para>
    /// <para>PostgreSQL data type: 'timestamp with time zone'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.TimestampTz'.</para>
    /// <para>CLR type: 'DateTime?'.</para>
    /// <para>linq2db data type: 'DataType.DateTime2'.</para>
    /// </summary>
    [Nullable]
    [Column(Name = "logout_date", DataType = DataType.DateTime2)]
    public DateTime? LogoutDate { get; set; }

    /// <summary>
    /// <para>Column name: 'profile_id'.</para>
    /// <para>Table name: 'user_sessions'.</para>
    /// <para>Foreign key column [public.user_sessions.profile_id -> public.user_profiles.user_profile_id].</para>
    /// <para>Foreign key constraint name: 'user_sessions_profile_id_fkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [LinqToDB.Mapping.NotNull]
    [Column(Name = "profile_id", DataType = DataType.Int32)]
    public int ProfileID { get; set; }

    /// <summary>
    /// <para>Column name: 'session_id'.</para>
    /// <para>Table name: 'user_sessions'.</para>
    /// <para>Primary key of table: 'user_sessions'.</para>
    /// <para>Primary key constraint name: 'user_sessions_pkey'.</para>
    /// <para>This column is not nullable.</para>
    /// <para>PostgreSQL data type: 'integer'.</para>
    /// <para>NpgsqlDbType: 'NpgsqlDbType.Integer'.</para>
    /// <para>CLR type: 'int'.</para>
    /// <para>linq2db data type: 'DataType.Int32'.</para>
    /// </summary>
    [PrimaryKey]
    [Identity]
    [Column(Name = "session_id", DataType = DataType.Int32)]
    public int SessionID { get; set; }

    public NpgsqlParameter[] GetNonPkParameters()
    {
        // ReSharper disable once RedundantExplicitArrayCreation
        return new NpgsqlParameter[]
        {
            new NpgsqlParameter<string>
            {
                TypedValue = this.CsrfTokenHash,
                NpgsqlDbType = NpgsqlDbType.Text,
            },
            new NpgsqlParameter<DateTime>
            {
                TypedValue = this.ExpirationDate,
                NpgsqlDbType = NpgsqlDbType.TimestampTz,
            },
            new NpgsqlParameter<bool>
            {
                TypedValue = this.LoggedOut,
                NpgsqlDbType = NpgsqlDbType.Boolean,
            },
            new NpgsqlParameter<DateTime>
            {
                TypedValue = this.LoginDate,
                NpgsqlDbType = NpgsqlDbType.TimestampTz,
            },
            new NpgsqlParameter<int>
            {
                TypedValue = this.LoginID,
                NpgsqlDbType = NpgsqlDbType.Integer,
            },
            this.LogoutDate.HasValue
                ? new NpgsqlParameter<DateTime> {TypedValue = this.LogoutDate.Value, NpgsqlDbType = NpgsqlDbType.TimestampTz}
                : new NpgsqlParameter {Value = DBNull.Value},
            new NpgsqlParameter<int>
            {
                TypedValue = this.ProfileID,
                NpgsqlDbType = NpgsqlDbType.Integer,
            },
        };
    }

    public int GetPrimaryKey()
    {
        return this.SessionID;
    }

    public void SetPrimaryKey(int value)
    {
        this.SessionID = value;
    }

    public bool IsNew()
    {
        return this.SessionID == default;
    }

    public async Task WriteToImporter(NpgsqlBinaryImporter importer)
    {
        if (this.CsrfTokenHash == null)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.CsrfTokenHash, NpgsqlDbType.Text);
        }

        await importer.WriteAsync(this.ExpirationDate, NpgsqlDbType.TimestampTz);

        await importer.WriteAsync(this.LoggedOut, NpgsqlDbType.Boolean);

        await importer.WriteAsync(this.LoginDate, NpgsqlDbType.TimestampTz);

        await importer.WriteAsync(this.LoginID, NpgsqlDbType.Integer);

        if (!this.LogoutDate.HasValue)
        {
            await importer.WriteNullAsync();
        }
        else
        {
            await importer.WriteAsync(this.LogoutDate.Value, NpgsqlDbType.TimestampTz);
        }

        await importer.WriteAsync(this.ProfileID, NpgsqlDbType.Integer);
    }

    public static TableMetadataModel Metadata => DbMetadata.UserSessionPocoMetadata;
}

public class DbPocos : IDbPocos<DbPocos>
{
    /// <summary>
    /// <para>Database table 'quiz_answers'.</para>
    /// </summary>
    public IQueryable<QuizAnswerPoco> QuizAnswers => this.LinqProvider.GetTable<QuizAnswerPoco>();

    /// <summary>
    /// <para>Database table 'quiz_questions'.</para>
    /// </summary>
    public IQueryable<QuizQuestionPoco> QuizQuestions => this.LinqProvider.GetTable<QuizQuestionPoco>();

    /// <summary>
    /// <para>Database table 'quizzes'.</para>
    /// </summary>
    public IQueryable<QuizPoco> Quizzes => this.LinqProvider.GetTable<QuizPoco>();

    /// <summary>
    /// <para>Database table 'user_logins'.</para>
    /// </summary>
    public IQueryable<UserLoginPoco> UserLogins => this.LinqProvider.GetTable<UserLoginPoco>();

    /// <summary>
    /// <para>Database table 'user_profiles'.</para>
    /// </summary>
    public IQueryable<UserProfilePoco> UserProfiles => this.LinqProvider.GetTable<UserProfilePoco>();

    /// <summary>
    /// <para>Database table 'user_sessions'.</para>
    /// </summary>
    public IQueryable<UserSessionPoco> UserSessions => this.LinqProvider.GetTable<UserSessionPoco>();

    public ILinqProvider LinqProvider { private get; set; }
}

public class DbMetadata
{
    internal static readonly TableMetadataModel QuizAnswerPocoMetadata;

    internal static readonly TableMetadataModel QuizQuestionPocoMetadata;

    internal static readonly TableMetadataModel QuizPocoMetadata;

    internal static readonly TableMetadataModel UserLoginPocoMetadata;

    internal static readonly TableMetadataModel UserProfilePocoMetadata;

    internal static readonly TableMetadataModel UserSessionPocoMetadata;

    // ReSharper disable once CollectionNeverQueried.Global
    internal static readonly List<FunctionMetadataModel> Functions = new List<FunctionMetadataModel>();

    // ReSharper disable once FunctionComplexityOverflow
    // ReSharper disable once CyclomaticComplexity
    static DbMetadata()
    {
        QuizAnswerPocoMetadata = new TableMetadataModel
        {
            ClassName = "QuizAnswer",
            PluralClassName = "QuizAnswers",
            TableName = "quiz_answers",
            TableSchema = "public",
            PrimaryKeyColumnName = "answer_id",
            PrimaryKeyPropertyName = "AnswerID",
            Columns = new List<ColumnMetadataModel>
            {
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "answer_content",
                    DbDataType = "text",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "AnswerContent",
                    TableName = "quiz_answers",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "answer_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("True"),
                    PrimaryKeyConstraintName = "quiz_answers_pkey" == string.Empty ? null : "quiz_answers_pkey",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "AnswerID",
                    TableName = "quiz_answers",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "answer_is_correct",
                    DbDataType = "boolean",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "AnswerIsCorrect",
                    TableName = "quiz_answers",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "bool",
                        ClrType = typeof(bool),
                        ClrNonNullableTypeName = "bool",
                        ClrNonNullableType = typeof(bool),
                        ClrNullableTypeName = "bool?",
                        ClrNullableType = typeof(bool?),
                        DbDataType = "boolean",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Boolean",
                        Linq2DbDataType = DataType.Boolean,
                        NpgsqlDbTypeName = "NpgsqlDbType.Boolean",
                        NpgsqlDbType = NpgsqlDbType.Boolean,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "answer_position",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "AnswerPosition",
                    TableName = "quiz_answers",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "question_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("True"),
                    ForeignKeyConstraintName = "quiz_answers_question_id_fkey" == string.Empty ? null : "quiz_answers_question_id_fkey",
                    ForeignKeyReferenceColumnName = "question_id" == string.Empty ? null : "question_id",
                    ForeignKeyReferenceSchemaName = "public" == string.Empty ? null : "public",
                    ForeignKeyReferenceTableName = "quiz_questions" == string.Empty ? null : "quiz_questions",
                    PropertyName = "QuestionID",
                    TableName = "quiz_answers",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
            },
            NonPkColumnNames = new[]
            {
                "answer_content",
                "answer_is_correct",
                "answer_position",
                "question_id",
            },
        };

        QuizQuestionPocoMetadata = new TableMetadataModel
        {
            ClassName = "QuizQuestion",
            PluralClassName = "QuizQuestions",
            TableName = "quiz_questions",
            TableSchema = "public",
            PrimaryKeyColumnName = "question_id",
            PrimaryKeyPropertyName = "QuestionID",
            Columns = new List<ColumnMetadataModel>
            {
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "question_content",
                    DbDataType = "text",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "QuestionContent",
                    TableName = "quiz_questions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "question_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("True"),
                    PrimaryKeyConstraintName = "quiz_questions_pkey" == string.Empty ? null : "quiz_questions_pkey",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "QuestionID",
                    TableName = "quiz_questions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "question_name",
                    DbDataType = "text",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "QuestionName",
                    TableName = "quiz_questions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "question_position",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "QuestionPosition",
                    TableName = "quiz_questions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "quiz_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("True"),
                    ForeignKeyConstraintName = "quiz_questions_quiz_id_fkey" == string.Empty ? null : "quiz_questions_quiz_id_fkey",
                    ForeignKeyReferenceColumnName = "quiz_id" == string.Empty ? null : "quiz_id",
                    ForeignKeyReferenceSchemaName = "public" == string.Empty ? null : "public",
                    ForeignKeyReferenceTableName = "quizzes" == string.Empty ? null : "quizzes",
                    PropertyName = "QuizID",
                    TableName = "quiz_questions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
            },
            NonPkColumnNames = new[]
            {
                "question_content",
                "question_name",
                "question_position",
                "quiz_id",
            },
        };

        QuizPocoMetadata = new TableMetadataModel
        {
            ClassName = "Quiz",
            PluralClassName = "Quizzes",
            TableName = "quizzes",
            TableSchema = "public",
            PrimaryKeyColumnName = "quiz_id",
            PrimaryKeyPropertyName = "QuizID",
            Columns = new List<ColumnMetadataModel>
            {
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "quiz_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("True"),
                    PrimaryKeyConstraintName = "quizzes_pkey" == string.Empty ? null : "quizzes_pkey",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "QuizID",
                    TableName = "quizzes",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "quiz_name",
                    DbDataType = "text",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "QuizName",
                    TableName = "quizzes",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "user_profile_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("True"),
                    ForeignKeyConstraintName = "quizzes_user_profile_id_fkey" == string.Empty ? null : "quizzes_user_profile_id_fkey",
                    ForeignKeyReferenceColumnName = "user_profile_id" == string.Empty ? null : "user_profile_id",
                    ForeignKeyReferenceSchemaName = "public" == string.Empty ? null : "public",
                    ForeignKeyReferenceTableName = "user_profiles" == string.Empty ? null : "user_profiles",
                    PropertyName = "UserProfileID",
                    TableName = "quizzes",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
            },
            NonPkColumnNames = new[]
            {
                "quiz_name",
                "user_profile_id",
            },
        };

        UserLoginPocoMetadata = new TableMetadataModel
        {
            ClassName = "UserLogin",
            PluralClassName = "UserLogins",
            TableName = "user_logins",
            TableSchema = "public",
            PrimaryKeyColumnName = "login_id",
            PrimaryKeyPropertyName = "LoginID",
            Columns = new List<ColumnMetadataModel>
            {
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "email_address",
                    DbDataType = "text",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "EmailAddress",
                    TableName = "user_logins",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "enabled",
                    DbDataType = "boolean",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "Enabled",
                    TableName = "user_logins",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "bool",
                        ClrType = typeof(bool),
                        ClrNonNullableTypeName = "bool",
                        ClrNonNullableType = typeof(bool),
                        ClrNullableTypeName = "bool?",
                        ClrNullableType = typeof(bool?),
                        DbDataType = "boolean",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Boolean",
                        Linq2DbDataType = DataType.Boolean,
                        NpgsqlDbTypeName = "NpgsqlDbType.Boolean",
                        NpgsqlDbType = NpgsqlDbType.Boolean,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "login_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("True"),
                    PrimaryKeyConstraintName = "user_logins_pkey" == string.Empty ? null : "user_logins_pkey",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "LoginID",
                    TableName = "user_logins",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "password_hash",
                    DbDataType = "text",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "PasswordHash",
                    TableName = "user_logins",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "user_profile_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("True"),
                    ForeignKeyConstraintName = "user_logins_user_profile_id_fkey" == string.Empty ? null : "user_logins_user_profile_id_fkey",
                    ForeignKeyReferenceColumnName = "user_profile_id" == string.Empty ? null : "user_profile_id",
                    ForeignKeyReferenceSchemaName = "public" == string.Empty ? null : "public",
                    ForeignKeyReferenceTableName = "user_profiles" == string.Empty ? null : "user_profiles",
                    PropertyName = "UserProfileID",
                    TableName = "user_logins",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "verification_code",
                    DbDataType = "text",
                    IsNullable = bool.Parse("True"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "VerificationCode",
                    TableName = "user_logins",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("True"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "verified",
                    DbDataType = "boolean",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "Verified",
                    TableName = "user_logins",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "bool",
                        ClrType = typeof(bool),
                        ClrNonNullableTypeName = "bool",
                        ClrNonNullableType = typeof(bool),
                        ClrNullableTypeName = "bool?",
                        ClrNullableType = typeof(bool?),
                        DbDataType = "boolean",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Boolean",
                        Linq2DbDataType = DataType.Boolean,
                        NpgsqlDbTypeName = "NpgsqlDbType.Boolean",
                        NpgsqlDbType = NpgsqlDbType.Boolean,
                    },
                },
            },
            NonPkColumnNames = new[]
            {
                "email_address",
                "enabled",
                "password_hash",
                "user_profile_id",
                "verification_code",
                "verified",
            },
        };

        UserProfilePocoMetadata = new TableMetadataModel
        {
            ClassName = "UserProfile",
            PluralClassName = "UserProfiles",
            TableName = "user_profiles",
            TableSchema = "public",
            PrimaryKeyColumnName = "user_profile_id",
            PrimaryKeyPropertyName = "UserProfileID",
            Columns = new List<ColumnMetadataModel>
            {
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "email_address",
                    DbDataType = "text",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "EmailAddress",
                    TableName = "user_profiles",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "registration_date",
                    DbDataType = "timestamp with time zone",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "RegistrationDate",
                    TableName = "user_profiles",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "DateTime",
                        ClrType = typeof(DateTime),
                        ClrNonNullableTypeName = "DateTime",
                        ClrNonNullableType = typeof(DateTime),
                        ClrNullableTypeName = "DateTime?",
                        ClrNullableType = typeof(DateTime?),
                        DbDataType = "timestamp with time zone",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.DateTime2",
                        Linq2DbDataType = DataType.DateTime2,
                        NpgsqlDbTypeName = "NpgsqlDbType.TimestampTz",
                        NpgsqlDbType = NpgsqlDbType.TimestampTz,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "user_profile_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("True"),
                    PrimaryKeyConstraintName = "user_profiles_pkey" == string.Empty ? null : "user_profiles_pkey",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "UserProfileID",
                    TableName = "user_profiles",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
            },
            NonPkColumnNames = new[]
            {
                "email_address",
                "registration_date",
            },
        };

        UserSessionPocoMetadata = new TableMetadataModel
        {
            ClassName = "UserSession",
            PluralClassName = "UserSessions",
            TableName = "user_sessions",
            TableSchema = "public",
            PrimaryKeyColumnName = "session_id",
            PrimaryKeyPropertyName = "SessionID",
            Columns = new List<ColumnMetadataModel>
            {
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "csrf_token_hash",
                    DbDataType = "text",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "CsrfTokenHash",
                    TableName = "user_sessions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "string",
                        ClrType = typeof(string),
                        ClrNonNullableTypeName = "string",
                        ClrNonNullableType = typeof(string),
                        ClrNullableTypeName = "string",
                        ClrNullableType = typeof(string),
                        DbDataType = "text",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("False"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.Text",
                        Linq2DbDataType = DataType.Text,
                        NpgsqlDbTypeName = "NpgsqlDbType.Text",
                        NpgsqlDbType = NpgsqlDbType.Text,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "expiration_date",
                    DbDataType = "timestamp with time zone",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "ExpirationDate",
                    TableName = "user_sessions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "DateTime",
                        ClrType = typeof(DateTime),
                        ClrNonNullableTypeName = "DateTime",
                        ClrNonNullableType = typeof(DateTime),
                        ClrNullableTypeName = "DateTime?",
                        ClrNullableType = typeof(DateTime?),
                        DbDataType = "timestamp with time zone",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.DateTime2",
                        Linq2DbDataType = DataType.DateTime2,
                        NpgsqlDbTypeName = "NpgsqlDbType.TimestampTz",
                        NpgsqlDbType = NpgsqlDbType.TimestampTz,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "logged_out",
                    DbDataType = "boolean",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "LoggedOut",
                    TableName = "user_sessions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "bool",
                        ClrType = typeof(bool),
                        ClrNonNullableTypeName = "bool",
                        ClrNonNullableType = typeof(bool),
                        ClrNullableTypeName = "bool?",
                        ClrNullableType = typeof(bool?),
                        DbDataType = "boolean",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Boolean",
                        Linq2DbDataType = DataType.Boolean,
                        NpgsqlDbTypeName = "NpgsqlDbType.Boolean",
                        NpgsqlDbType = NpgsqlDbType.Boolean,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "login_date",
                    DbDataType = "timestamp with time zone",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "LoginDate",
                    TableName = "user_sessions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "DateTime",
                        ClrType = typeof(DateTime),
                        ClrNonNullableTypeName = "DateTime",
                        ClrNonNullableType = typeof(DateTime),
                        ClrNullableTypeName = "DateTime?",
                        ClrNullableType = typeof(DateTime?),
                        DbDataType = "timestamp with time zone",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.DateTime2",
                        Linq2DbDataType = DataType.DateTime2,
                        NpgsqlDbTypeName = "NpgsqlDbType.TimestampTz",
                        NpgsqlDbType = NpgsqlDbType.TimestampTz,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "login_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("True"),
                    ForeignKeyConstraintName = "user_sessions_login_id_fkey" == string.Empty ? null : "user_sessions_login_id_fkey",
                    ForeignKeyReferenceColumnName = "login_id" == string.Empty ? null : "login_id",
                    ForeignKeyReferenceSchemaName = "public" == string.Empty ? null : "public",
                    ForeignKeyReferenceTableName = "user_logins" == string.Empty ? null : "user_logins",
                    PropertyName = "LoginID",
                    TableName = "user_sessions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "logout_date",
                    DbDataType = "timestamp with time zone",
                    IsNullable = bool.Parse("True"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "LogoutDate",
                    TableName = "user_sessions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "DateTime?",
                        ClrType = typeof(DateTime?),
                        ClrNonNullableTypeName = "DateTime",
                        ClrNonNullableType = typeof(DateTime),
                        ClrNullableTypeName = "DateTime?",
                        ClrNullableType = typeof(DateTime?),
                        DbDataType = "timestamp with time zone",
                        IsNullable = bool.Parse("True"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("True"),
                        IsClrReferenceType = bool.Parse("True"),
                        Linq2DbDataTypeName = "DataType.DateTime2",
                        Linq2DbDataType = DataType.DateTime2,
                        NpgsqlDbTypeName = "NpgsqlDbType.TimestampTz",
                        NpgsqlDbType = NpgsqlDbType.TimestampTz,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "profile_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("False"),
                    PrimaryKeyConstraintName = "" == string.Empty ? null : "",
                    IsForeignKey = bool.Parse("True"),
                    ForeignKeyConstraintName = "user_sessions_profile_id_fkey" == string.Empty ? null : "user_sessions_profile_id_fkey",
                    ForeignKeyReferenceColumnName = "user_profile_id" == string.Empty ? null : "user_profile_id",
                    ForeignKeyReferenceSchemaName = "public" == string.Empty ? null : "public",
                    ForeignKeyReferenceTableName = "user_profiles" == string.Empty ? null : "user_profiles",
                    PropertyName = "ProfileID",
                    TableName = "user_sessions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
                new ColumnMetadataModel
                {
                    ColumnComment = "" == string.Empty ? null : "",
                    Comments = "".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries),
                    ColumnName = "session_id",
                    DbDataType = "integer",
                    IsNullable = bool.Parse("False"),
                    IsPrimaryKey = bool.Parse("True"),
                    PrimaryKeyConstraintName = "user_sessions_pkey" == string.Empty ? null : "user_sessions_pkey",
                    IsForeignKey = bool.Parse("False"),
                    ForeignKeyConstraintName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceColumnName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceSchemaName = "" == string.Empty ? null : "",
                    ForeignKeyReferenceTableName = "" == string.Empty ? null : "",
                    PropertyName = "SessionID",
                    TableName = "user_sessions",
                    TableSchema = "public",
                    PropertyType = new SimpleType
                    {
                        ClrTypeName = "int",
                        ClrType = typeof(int),
                        ClrNonNullableTypeName = "int",
                        ClrNonNullableType = typeof(int),
                        ClrNullableTypeName = "int?",
                        ClrNullableType = typeof(int?),
                        DbDataType = "integer",
                        IsNullable = bool.Parse("False"),
                        IsClrValueType = bool.Parse("True"),
                        IsClrNullableType = bool.Parse("False"),
                        IsClrReferenceType = bool.Parse("False"),
                        Linq2DbDataTypeName = "DataType.Int32",
                        Linq2DbDataType = DataType.Int32,
                        NpgsqlDbTypeName = "NpgsqlDbType.Integer",
                        NpgsqlDbType = NpgsqlDbType.Integer,
                    },
                },
            },
            NonPkColumnNames = new[]
            {
                "csrf_token_hash",
                "expiration_date",
                "logged_out",
                "login_date",
                "login_id",
                "logout_date",
                "profile_id",
            },
        };

    }
}
