import { RpcClientHelper } from '~infrastructure/RpcClientHelper';
import { Result } from '~infrastructure/helpers';

export interface LoginRequest {
  username: string;
  password: string;
  rememberMe: boolean;
}

export interface RegisterRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  csrfToken: string;
  emailAddress: string;
  userProfileID: number;
}

export interface PingResponse {
  message: string;
}

export interface RegisterResponse {
  csrfToken: string;
  emailAddress: string;
  userProfileID: number;
}

export class RpcClient {
  helper: RpcClientHelper;

  constructor(helper: RpcClientHelper) {
    this.helper = helper;
  }

  login(request: LoginRequest): Promise<Result<LoginResponse>> {
    return this.helper.send('LoginRequest', request);
  }

  ping(): Promise<Result<PingResponse>> {
    return this.helper.send('PingRequest');
  }

  profileInfo(): Promise<Result> {
    return this.helper.send('ProfileInfoRequest');
  }

  register(request: RegisterRequest): Promise<Result<RegisterResponse>> {
    return this.helper.send('RegisterRequest', request);
  }
}
