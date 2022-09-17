// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable prettier/prettier */
import { BaseRpcClient } from './BaseRpcClient';
import { ApiResult } from './api-result';

export interface DeleteQuizRequest {
  id: number;
}

export interface GetQuizRequest {
  id: number;
}

export interface LoginRequest {
  emailAddress: string;
  password: string;
  rememberMe: boolean;
  recaptchaToken: string;
}

export interface ProfileInfoRequest {
  count: number;
}

export interface RegisterRequest {
  emailAddress: string;
  password: string;
  recaptchaToken: string;
}

export interface SaveQuizRequest {
  item: QuizModel;
}

export interface QuizModel {
  questions: QuizQuestionModel[];
  quizID: number;
  quizName: string;
  userProfileID: number;
}

export interface QuizQuestionModel {
  answers: QuizAnswerPoco[];
  questionContent: string;
  questionID: number;
  questionName: string;
  questionPosition: number;
  quizID: number;
}

export interface QuizAnswerPoco {
  answerContent: string;
  answerID: number;
  answerIsCorrect: boolean;
  answerPosition: number;
  questionID: number;
}

export interface SearchQuizzesRequest {
  query: string;
}

export interface GetQuizResponse {
  item: QuizModel;
}

export interface LoginResponse {
  csrfToken: string;
  emailAddress: string;
  userProfileID: number;
}

export interface PingResponse {
  message: string;
}

export interface ProfileInfoResponse {
  count: number;
}

export interface SearchQuizzesResponse {
  items: QuizPoco[];
}

export interface QuizPoco {
  quizID: number;
  quizName: string;
  userProfileID: number;
}

export interface DefaultApiError {
  errorMessages: string[];
  errorID?: string;
}

export interface LoginError {
  userNotVerified?: boolean;
  errorMessages: string[];
  errorID?: string;
}

export class RpcClient {
  private baseClient: BaseRpcClient;

  constructor(baseClient: BaseRpcClient) {
    this.baseClient = baseClient;
  }

  deleteQuiz(request: DeleteQuizRequest): Promise<void> {
    return this.baseClient.send('DeleteQuizRequest', request);
  }

  deleteQuizResult(request: DeleteQuizRequest): Promise<ApiResult> {
    return this.baseClient.sendResult('DeleteQuizRequest', request);
  }

  getQuiz(request: GetQuizRequest): Promise<GetQuizResponse> {
    return this.baseClient.send('GetQuizRequest', request);
  }

  getQuizResult(request: GetQuizRequest): Promise<ApiResult<GetQuizResponse>> {
    return this.baseClient.sendResult('GetQuizRequest', request);
  }

  login(request: LoginRequest): Promise<LoginResponse> {
    return this.baseClient.send('LoginRequest', request);
  }

  loginResult(request: LoginRequest): Promise<ApiResult<LoginResponse, LoginError>> {
    return this.baseClient.sendResult('LoginRequest', request);
  }

  logout(): Promise<void> {
    return this.baseClient.send('LogoutRequest');
  }

  logoutResult(): Promise<ApiResult> {
    return this.baseClient.sendResult('LogoutRequest');
  }

  ping(): Promise<PingResponse> {
    return this.baseClient.send('PingRequest');
  }

  pingResult(): Promise<ApiResult<PingResponse>> {
    return this.baseClient.sendResult('PingRequest');
  }

  profileInfo(request: ProfileInfoRequest): Promise<ProfileInfoResponse> {
    return this.baseClient.send('ProfileInfoRequest', request);
  }

  profileInfoResult(request: ProfileInfoRequest): Promise<ApiResult<ProfileInfoResponse>> {
    return this.baseClient.sendResult('ProfileInfoRequest', request);
  }

  register(request: RegisterRequest): Promise<void> {
    return this.baseClient.send('RegisterRequest', request);
  }

  registerResult(request: RegisterRequest): Promise<ApiResult> {
    return this.baseClient.sendResult('RegisterRequest', request);
  }

  saveQuiz(request: SaveQuizRequest): Promise<void> {
    return this.baseClient.send('SaveQuizRequest', request);
  }

  saveQuizResult(request: SaveQuizRequest): Promise<ApiResult> {
    return this.baseClient.sendResult('SaveQuizRequest', request);
  }

  searchQuizzes(request: SearchQuizzesRequest): Promise<SearchQuizzesResponse> {
    return this.baseClient.send('SearchQuizzesRequest', request);
  }

  searchQuizzesResult(request: SearchQuizzesRequest): Promise<ApiResult<SearchQuizzesResponse>> {
    return this.baseClient.sendResult('SearchQuizzesRequest', request);
  }
}
