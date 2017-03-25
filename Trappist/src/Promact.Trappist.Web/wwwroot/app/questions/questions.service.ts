import { Injectable } from "@angular/core";
import { HttpService } from "../core/http.service";
@Injectable()
export class QuestionsService {
    private questionsApiUrl = "api";
    constructor(private httpService: HttpService) { }
    /**
     * get list of questions
     */
    getQuestions() {
        return this.httpService.get(this.questionsApiUrl);
    }

    /**
     * gets list of coding languages
     */
    getCodingLanguage() {
        return this.httpService.get(this.questionsApiUrl + "/codinglanguage");
    }

    postCodingQuestion() {

    }
}