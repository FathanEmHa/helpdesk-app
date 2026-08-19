export class ApiError extends Error {
  statusCode: number;
  errors: string[];

  constructor(
    message: string,
    statusCode: number,
    errors: string[] = [],
  ) {
    super(message);

    this.name = "ApiError";
    this.statusCode = statusCode;
    this.errors = errors;
  }
}