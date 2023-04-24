import {GradeInterface} from "../Grade/Grade";

export default function CalculateAverage(...grades: GradeInterface[]): number | string {
    let sumgrades: number = 0;
    let sumweight: number = 0;
    grades.forEach((item: GradeInterface) => {
        sumgrades += item.grade * item.weight
        sumweight += item.weight
    })
    if (sumgrades === 0) return "No grades insert yet"
    return Math.round(((sumgrades / sumweight) + Number.EPSILON) * 100) / 100
}