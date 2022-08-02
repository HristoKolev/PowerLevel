
drop schema if exists "public" cascade;
create schema "public";

create table public.quizzes (
	quiz_id serial,
	
	quiz_name text not null,
	
	primary key (quiz_id)
);

create table public.quiz_questions (
	question_id serial,
	
	question_name text not null,
	question_content text not null,
	
	quiz_id int not null references quizzes,

	primary key (question_id)
);

create table public.quiz_answers (
    answer_id serial,

    answer_content text not null,
    answer_is_correct boolean not null,
    
    question_id int not null references quiz_questions,

    primary key (answer_id)
);

create table public.user_profiles (
    user_profile_id serial,

    email_address text not null,
    registration_date timestamptz(0)    not null,

    primary key (user_profile_id)
);

create table public.user_logins (
    login_id serial,

    enabled boolean not null,

    email_address text not null,
    password_hash text not null,

    verification_code text,
    verified bool not null,

    user_profile_id int not null references user_profiles,

    primary key (login_id)
);

create table public.user_sessions (
    session_id serial,

    login_date timestamptz(0) not null,
    expiration_date timestamptz(0) not null,
    csrf_token_hash text not null,
    logged_out boolean not null,
    logout_date timestamptz(0),

    login_id int not null references user_logins,
    profile_id int not null references user_profiles,

    primary key (session_id)
);
