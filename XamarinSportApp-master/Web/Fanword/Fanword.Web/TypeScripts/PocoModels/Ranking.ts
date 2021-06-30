class Ranking {
    id: string;
    sportId: string;
    rankingTeams: RankingTeam[];

    constructor() {
        this.rankingTeams = [];
    }
}